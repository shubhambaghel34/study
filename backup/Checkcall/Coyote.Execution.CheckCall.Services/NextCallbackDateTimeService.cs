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

namespace Coyote.Execution.CheckCall.Services
{
    using System;
    using log4net;
    using Newtonsoft.Json;
    using Coyote.Execution.CheckCall.Contracts;
    using Coyote.Execution.CheckCall.Contracts.Services;
    using Coyote.Common.Extensions;

    public class NextCallbackDateTimeService : ServiceBase , INextCallbackDateTimeService
    {

        public NextCallbackDateTimeService(ILog logger)
            :base(logger, "NextCallbackDateTimeServiceUri")
        { }

        public DateTime? GetNextCallbackDateTime(int auditUserId, LoadInfo load)
        {
            load.ThrowIfNull("load");

            Logger.InfoFormat("Getting Next Callback DateTime for Load : {0}", load.LoadId);
            
            var result = GetRaw(auditUserId, string.Format("/GetNextCallbackDateTime?loadId={0}&progressType={1}&checkInDateTime={2}", load.LoadId, 2, DateTime.Now.ToString()));

            return JsonConvert.DeserializeObject<DateTime?>(result);
        }
    }
}
