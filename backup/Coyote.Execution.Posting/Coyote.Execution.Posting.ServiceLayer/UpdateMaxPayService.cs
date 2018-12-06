// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2018 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////

namespace Coyote.Execution.Posting.ServiceLayer
{
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Contracts.Storage;
    using log4net;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class UpdateMaxPayService : ExternalServiceBase, IUpdateMaxPayService
    {
        private IRuntimeSettings _runtimeSettings;
        public UpdateMaxPayService(ILog log, IPostingRepository postingRepository, IRuntimeSettings runtimeSettings)
            : base(postingRepository, new Uri(runtimeSettings?.UpdateMaxPayWebUrl), Common.Coyote.Types.ExternalService.Undefined, log)
        {
            _runtimeSettings = runtimeSettings.ThrowIfArgumentNull(nameof(runtimeSettings));
        }

        public async Task UpdateMaxPayAsync(int loadId, decimal postedRate)
        {
            loadId.ThrowIfArgumentLessThanOrEqualTo(nameof(loadId), 0);

            var postLoad = new
            {
                LoadId = loadId,
                PostedRate = postedRate
            };

           await PostContentAsync(JsonConvert.SerializeObject(postLoad),
                                   "v1/UpdateMaxPay",
                                   _runtimeSettings.ServiceUserId);
        }
    }
}
