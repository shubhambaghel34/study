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
    using Coyote.Execution.Posting.Storage;
    using ServiceControl.Plugin.CustomChecks;
    using System;
    using System.Threading.Tasks;

#pragma warning disable CS3009
    public class DatabaseCheck : CustomCheck
    {
        #region " Private fields "
        private readonly Database _sqlDatabase;
        #endregion

        #region " Constructor "
        public DatabaseCheck(Database sqlDatabase)
            : base("Coyote Execution Posting - Endpoint - DataBase", "Database", TimeSpan.FromMinutes(2))
        {
            _sqlDatabase = sqlDatabase ?? throw new ArgumentNullException(nameof(sqlDatabase));
        }
        #endregion

        #region " Publice methods "
#pragma warning disable CS3002
        public override async Task<CheckResult> PerformCheck()
        {
            var result = await _sqlDatabase.TryConnect();
            
            if (!result.Item1)
            {
                return CheckResult.Pass;
            }
            return CheckResult.Failed(result.Item2);
        }
#pragma warning restore CS3002
        #endregion
    }
#pragma warning restore CS3009
}