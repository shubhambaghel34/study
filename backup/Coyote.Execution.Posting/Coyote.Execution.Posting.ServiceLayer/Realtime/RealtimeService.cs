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
namespace Coyote.Execution.Posting.ServiceLayer.Realtime
{
    using Coyote.Common.Finance;
    using Coyote.Execution.Posting.Common.Coyote.Types;
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts;
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.ServiceLayer;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Coyote.Systems.Bazooka.Realtime.Contracts.Enums;
    using Coyote.Systems.Bazooka.Realtime.Contracts.Tracers;
    using Coyote.Systems.Bazooka.Realtime.Info.Contracts;
    using log4net;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class RealtimeService: ExternalServiceBase, IRealtimeService
    {
        #region " Constructor "
        public RealtimeService(IPostingRepository postingRepository, IRuntimeSettings runtimeSettings, ILog log)
           : base(postingRepository, new Uri(runtimeSettings?.RealtimeUpdateServiceAddress), ExternalService.Undefined, log)
        {
            runtimeSettings.ThrowIfArgumentNull(nameof(runtimeSettings));
        }
        #endregion

        #region " Public Methods "
        public async Task SendRealtimeUpdatesAsync(LoadPostBase loadPostBase)
        {
            loadPostBase.ThrowIfArgumentNull(nameof(loadPostBase));

            await PostContentAsync(BuildRealtimeContent(loadPostBase),
                                    "RealtimeUpdate",
                                    loadPostBase.UserId);
        }
        #endregion

        #region " Private Methods "
        private static string BuildRealtimeContent(LoadPostBase loadPostBase)
        {
            return JsonConvert.SerializeObject(BuildRealtimeTracer(loadPostBase), new JsonSerializerSettings()
            {
                ContractResolver = new DefaultContractResolver(),
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        private static RealtimeTracer BuildRealtimeTracer(LoadPostBase loadPostBase)
        {
            var origin = new Stack<IAddressNode>();
            origin.Push(new AddressNode());

            return new RealtimeTracer
            {   
                Domain = (int)Domain.BazookaRealtimeUpdating,
                Command = (int)BazookaRealtimeUpdating.Load,
                Data = BuildRealTimeLoadInfo(loadPostBase),
                Origin = origin,
                Version = 2.0M
            };
        }

        private static Dictionary<string, object> BuildRealTimeLoadInfo(LoadPostBase loadPostBase)
        {
            var LoadPostedExternally = (loadPostBase.PostEverywherePostStatus == (int)ExternalLoadPostStatus.Posted ||
                            loadPostBase.DATPostStatus == (int)ExternalLoadPostStatus.Posted ||
                            loadPostBase.ITSPostStatus == (int)ExternalLoadPostStatus.Posted);
            Dictionary<string, object> dicRealTimeLoadInfo = new Dictionary<string, object>();

            dicRealTimeLoadInfo.Add(nameof(IRealTimeLoadInfo.LoadID), loadPostBase.LoadId);
            dicRealTimeLoadInfo.Add(nameof(IRealTimeLoadInfo.PostedRate), new MoneyBind(CurrencyType.USD, loadPostBase.Rate));
            dicRealTimeLoadInfo.Add(nameof(IRealTimeLoadInfo.LoadPostedExternally), LoadPostedExternally);

            return dicRealTimeLoadInfo;
        }
        #endregion
    }
}