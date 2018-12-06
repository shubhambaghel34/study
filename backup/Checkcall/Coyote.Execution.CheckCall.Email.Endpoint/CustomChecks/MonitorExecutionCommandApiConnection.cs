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
    using System.Net.Http;
    using ServiceControl.Plugin.CustomChecks;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Api")]
    public class MonitorExecutionCommandApiConnection : PeriodicCheck
    {
        public MonitorExecutionCommandApiConnection()
            : base("Daily Check Call Email - Execution Command API connection", "Network resources", TimeSpan.FromMinutes(5))
        { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This is for Service Pulse we want to log any exception")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", Justification = "This is for Service Pulse we want to log any exception")]
        public override CheckResult PerformCheck()
        {
            var url = $"{ConfigurationManager.AppSettings["ExecutionCommandServiceUri"]}/help";
            using (var client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return CheckResult.Pass;
                    }
                }
            }

            return CheckResult.Failed($"Unable to contact Execution Command Core Service at : '{url}'");
        }

    }
}
