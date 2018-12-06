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

namespace Coyote.Execution.CheckCall.Email.Endpoint.CustomChecks
{
    using System;
    using System.Configuration;
    using ServiceControl.Plugin.CustomChecks;
    using Storage.Repositories;
    public class MonitorDbConnection : PeriodicCheck
    {
        private IAuditUserRepository AuditUserRepository { get; set; }

        public MonitorDbConnection(IAuditUserRepository auditUserRepository)
            : base("Daily Check Call Email Bazooka Database", "Network resources", TimeSpan.FromMinutes(5))
        {
            AuditUserRepository = auditUserRepository;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This is for Service Pulse we want to log any exception")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", Justification = "This is for Service Pulse we want to log any exception")]
        public override CheckResult PerformCheck()
        {
            try
            {
                var result = AuditUserRepository.GetAuditUserId("Coyote.Execution.CheckCall").Result;
            }
            catch (Exception ex)
            {
                return CheckResult.Failed($"Error contacting '{ConfigurationManager.ConnectionStrings["BazookaDbContext"]}'\r\n{ex.GetType().Name}: {ex.Message}");
            }

            return CheckResult.Pass;
        }
    }
}
