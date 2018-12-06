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
namespace Coyote.Execution.Posting.Endpoint.CustomChecks
{
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using ServiceControl.Plugin.CustomChecks;
    using System;
    using System.Threading.Tasks;

#pragma warning disable CS3009
    public class InternetTruckStopExternalServiceCheck : CustomCheck
    {
        #region " Private fields "
        private readonly IInternetTruckStopExternalService _internetTruckStopExternalService;
        #endregion

        #region " Constructor "
        public InternetTruckStopExternalServiceCheck(IInternetTruckStopExternalService internetTruckStopExternalService)
            : base("Coyote Execution Posting - Endpoint - InternetTruckStop", "HTTP Service", TimeSpan.FromMinutes(2))
        {
            _internetTruckStopExternalService = internetTruckStopExternalService ?? throw new ArgumentNullException(nameof(internetTruckStopExternalService));
        }
        #endregion

        #region " Publice methods "
#pragma warning disable CS3002
        public override async Task<CheckResult> PerformCheck()
        {
            var result = await _internetTruckStopExternalService.PingAsync();

            if (result)
            {
                return CheckResult.Pass;
            }
            return CheckResult.Failed("Ping of InternetTruckStop external service failed.");
        }
#pragma warning restore CS3002
        #endregion
    }
#pragma warning restore CS3009
}