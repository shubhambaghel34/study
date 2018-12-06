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
    using System.Linq;
    using log4net;
    using Newtonsoft.Json;
    using Coyote.Execution.CheckCall.Contracts.Services;
    using Coyote.Execution.CheckCall.Domain;
    using Coyote.Common.Extensions;

    public class EmailApi : ServiceBase, IEmailApi
    {
        public EmailApi(ILog logger)
            :base(logger, "EmailSystemUri")
        { }

        public bool SendEmail(int auditUserId, EmailMessage message)
        {
            message.ThrowIfNull("message");

            Logger.InfoFormat("Sending Email to SystemAPI - To : {0}", string.Join(";", message.To.Keys.ToArray()));

            PostContent(auditUserId, "/Emails", JsonConvert.SerializeObject(message));

            return true;
        }

    }
}
