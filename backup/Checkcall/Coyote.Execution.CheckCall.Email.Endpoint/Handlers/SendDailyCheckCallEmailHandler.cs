// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2018
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
    using Coyote.Execution.CheckCall.Email.Endpoint.Email;
    using log4net;
    using NServiceBus;
    using Storage.Repositories;

    public class SendDailyCheckCallEmailHandler : IHandleMessages<SendDailyCheckCallEmail>
    {
        private ILog Log { get; set; }
        private IBus Bus { get; set; }
        private ICarrierRepository CarrierRepository { get; set; }
        private IAuditUserRepository AuditUserRepository { get; set; }
        private IEmailApi EmailApi { get; set; }

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

        public SendDailyCheckCallEmailHandler(
            ILog log,
            IBus bus,
            ICarrierRepository carrierRepository,
            IAuditUserRepository auditUserRepository,
            IEmailApi emailApi)
        {
            Log = log.ThrowIfNull(nameof(log));
            Bus = bus.ThrowIfNull(nameof(bus));
            CarrierRepository = carrierRepository.ThrowIfNull(nameof(carrierRepository));
            AuditUserRepository = auditUserRepository.ThrowIfNull(nameof(auditUserRepository));
            EmailApi = emailApi.ThrowIfNull(nameof(emailApi));
        }

        public void Handle(SendDailyCheckCallEmail message)
        {
            message.ThrowIfNull(nameof(message));

            Log.InfoFormat($"SendDailyCheckCallEmailHandler - Processing Started SendDailyCheckCallEmail for : Carrier {message.CarrierId} - Date {message.Date.ToString("u")}");

            if (!message.Loads.Any())
            {
                Log.InfoFormat($"SendDailyCheckCallEmailHandler - Invalid Command Received - Daily Check Call Not Sent CarrierId : {message.CarrierId} - Date : {message.Date.ToString("MM/DD/YYYY")}");
                return;
            }

            var carrier = CarrierRepository.GetById(message.CarrierId).Result;

            var email = new DailyCheckCall(carrier, message);

            if (!string.IsNullOrEmpty(email.From) && EmailApi.SendEmail(AuditUserId, email))
            {
                Log.InfoFormat(
                    $"SendDailyCheckCallEmailHandler - Daily Check Call Email Sent for : Carrier {message.CarrierId} - Date {message.Date.ToString("u")}");

                var to = string.Join(";", email.To.Keys);
                message.Loads.ForEach(load => Bus.Send(new CreateTrackingNote(to, load, carrier.Id)));

                Log.InfoFormat($"SendDailyCheckCallEmailHandler - Processing Completed SendDailyCheckCallEmail for : Carrier {message.CarrierId} - Date {message.Date.ToString("u")}");
                return;
            }

            Log.InfoFormat($"SendDailyCheckCallEmailHandler - Daily Check Call Not Sent CarrierId : {message.CarrierId} - Date : {message.Date.ToString("MM/DD/YYYY")}");
        }

    }
}
