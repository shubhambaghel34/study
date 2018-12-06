// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.CheckCall.Endpoint.Handlers
{
    using Coyote.Business.Events.Publisher.Contracts;
    using Coyote.Business.Events.Publisher.Contracts.Load.Events;
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using Coyote.Execution.CheckCall.Storage.Repositories;
    using Coyote.Common.Extensions;
    using log4net;
    using NServiceBus;
    using System.Linq;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class LoadStateChangedEventHandler : IHandleMessages<ILoadStateChangedEvent>
    {
        private ILog Log { get; set; }
        private IBus Bus { get; set; }
        private ILoadRepository LoadRepository { get; set; }
        private ICarrierRepository CarrierRepository { get; set; }
        private ISystemSettingRepository SystemSettingRepository { get; set; }

        private const string CARRCONTRACTVERSION = "tms file";

        public LoadStateChangedEventHandler(ILog log, IBus bus, ILoadRepository loadRepository, ICarrierRepository carrierRepository, ISystemSettingRepository systemSettingRepository)
        {
            Log = log.ThrowIfNull(nameof(log));
            Bus = bus.ThrowIfNull(nameof(bus));
            LoadRepository = loadRepository.ThrowIfNull(nameof(loadRepository));
            CarrierRepository = carrierRepository.ThrowIfNull(nameof(carrierRepository));
            SystemSettingRepository = systemSettingRepository.ThrowIfNull(nameof(systemSettingRepository));
        }

        public void Handle(ILoadStateChangedEvent message)
        {
            message.ThrowIfNull(nameof(message));

            Log.InfoFormat($"LoadStateChangedEvent handled for Load {message.LoadId}.");

            if ((message.LoadState == LoadState.Cancelled) ||
               ((message.PreviousLoadState == LoadState.Hold || message.PreviousLoadState == LoadState.Cancelled) && (message.LoadState == LoadState.Active)))
            {
                var load = LoadRepository.GetById(message.LoadId).Result;
                if (load == null) return;

                string doNotReplyFromAddress = SystemSettingRepository.GetBySettingName("DoNotReplyFromAddress").Result?.SettingValue;
                doNotReplyFromAddress.ThrowIfNull(nameof(doNotReplyFromAddress));

                if (load.MainLoadCarrier == null) return;
                if (load.MainLoadCustomer == null) return;
                if (load.MainLoadCarrier.LoadReps.Any())
                {
                    var carrierContractVersion = CarrierRepository.GetById(load.MainLoadCarrier.CarrierId).Result.ContractVersion.Trim().ToLower();
                    if (!carrierContractVersion.Equals(CARRCONTRACTVERSION))
                    {
                        Bus.Send(new LoadActivationCancellationEmailCommand(load, doNotReplyFromAddress));
                        Log.InfoFormat("LoadActivationCancellationEmail command sent.");
                    }
                }
            }
        }
    }

}
