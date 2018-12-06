// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2017
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////


namespace Coyote.Execution.CheckCall.Email.Endpoint.Processor.Command
{
    using System;
    using System.Linq;
    using Common.Extensions;
    using Contracts.Services;
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using log4net;
    using NServiceBus;
    using Storage.Repositories;

    public class CreateTrackingNoteHandler : IHandleMessages<CreateTrackingNote>
    {
        private ILog Log { get; set; }
        private ICarrierRepository CarrierRepository { get; set; }
        private IAuditUserRepository AuditUserRepository { get; set; }
        private INextCallbackDateTimeService NextCallbackDateTimeService { get; set; }
        private IExecutionCommandService ExecutionCommandService { get; set; }

        private int _auditUserId = 0;
        private int AuditUserId
        {
            get
            {
                if (_auditUserId == 0)
                {
                    _auditUserId = AuditUserRepository.GetAuditUserId("Coyote.Execution.CheckCall").Result;
                }
                return _auditUserId;
            }
        }

        public CreateTrackingNoteHandler(
            ILog log,
            ICarrierRepository carrierRepository,
            IAuditUserRepository auditUserRepository,
            INextCallbackDateTimeService nextCallbackDateTimeService,
            IExecutionCommandService executionCommandService)
        {
            Log = log.ThrowIfNull("log");
            CarrierRepository = carrierRepository.ThrowIfNull("carrierRepository");
            AuditUserRepository = auditUserRepository.ThrowIfNull("auditUserRepository");
            NextCallbackDateTimeService = nextCallbackDateTimeService.ThrowIfNull("nextCallbackDateTimeService");
            ExecutionCommandService = executionCommandService.ThrowIfNull("executionCommandService");
        }

        public void Handle(CreateTrackingNote message)
        {
            if (message == null) throw new ArgumentNullException("message");

            Log.InfoFormat("CreateTrackingNoteHandler - Processing Started CreateTrackingNote for : Carrier {0}, LoadId {1}", message.CarrierId, message.LoadInfo.LoadId);

            var carrier = CarrierRepository.GetById(message.CarrierId).Result;

            var mainRep = carrier.Reps.FirstOrDefault(r => r.Main == true);
            if (mainRep != null)
            {
                DateTime? nextCallbackDateTime = NextCallbackDateTimeService.GetNextCallbackDateTime(AuditUserId, message.LoadInfo);
                ExecutionCommandService.CreateLoadTrackingNote(AuditUserId, message.LoadInfo, message.CarrierId, mainRep.Id, message.To, nextCallbackDateTime);

                Log.InfoFormat("CreateTrackingNoteHandler - Processing Ended CreateTrackingNote for : Carrier {0}, LoadId {1}", message.CarrierId, message.LoadInfo.LoadId);
            }

        }

    }
}
