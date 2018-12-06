// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2017
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
    using Coyote.Execution.CheckCall.Contracts.Commands;
    using Coyote.Execution.CheckCall.Contracts.Services;
    using Coyote.Execution.CheckCall.Storage.Repositories;
    using Coyote.Execution.CheckCall.Email.Endpoint.Email;
    using Coyote.Common.Extensions;
    using log4net;
    using NServiceBus;

    public class LoadActivationCancellationEmailCommandHandler : IHandleMessages<LoadActivationCancellationEmailCommand>
    {
        private ILog Log { get; set; }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Api")]
        public LoadActivationCancellationEmailCommandHandler(
          ILog log,
          IAuditUserRepository auditUserRepository,
          IEmailApi emailApi)
        {
            Log = log.ThrowIfNull(nameof(log));
            AuditUserRepository = auditUserRepository.ThrowIfNull(nameof(auditUserRepository));
            EmailApi = emailApi.ThrowIfNull(nameof(emailApi));
        }

        public void Handle(LoadActivationCancellationEmailCommand message)
        {
            message.ThrowIfNull(nameof(message));
            Log.InfoFormat($"LoadActivationCancellationEmailCommand handled for Load {message.LoadId}.");

            var email = new LoadActivationCancellationEmail(message);

            if (email != null)
            {                           
                if (EmailApi.SendEmail(AuditUserId, email))
                {
                    Log.InfoFormat($"LoadActivationCancellationEmail sent for Load {message.LoadId}");
                    return;
                }
                Log.InfoFormat($"LoadActivationCancellationEmail not sent for Load {message.LoadId}");
            }
        }
    }
}

