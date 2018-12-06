// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2017 - 2018
//                            Coyote Logistics L.L.C.
//                          All Rights Reserved Worldwide
// 
// WARNING:  This program (or document) is unpublished, proprietary
// property of Coyote Logistics L.L.C. and is to be maintained in strict confidence.
// Unauthorized reproduction, distribution or disclosure of this program
// (or document), or any program (or document) derived from it is
// prohibited by State and Federal law, and by local law outside of the U.S.
// /////////////////////////////////////////////////////////////////////////////////////
namespace Coyote.Execution.Posting.Web.Api.Controllers.ServiceMonitoring
{
    using System.Web.Http;

    /// <summary>
    /// Service Monitor Nudge
    /// </summary>
    public class NudgeController :
         ApiController
    {
        /// <summary>
        /// Used by service monitoring to determine if service is running and connected to the database.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [HttpGet, AllowAnonymous]
        public bool Get()
        {
            return true;
        }
    }
}