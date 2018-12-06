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
namespace Coyote.Execution.Posting.Tests.Integration.Managers
{
    using Coyote.Execution.Posting.Common.Extensions;
    using Dapper;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public static class ExternalLoadPostRepositoryManager
    {
        public static int GetValidLoadIdToRetrieveExternalLoadPost(string connectionString)
        {
            connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));

            using (var connection = new SqlConnection(connectionString))
            {
                string query = $@"	SELECT TOP 1 LoadId 
                                    FROM         [dbo].[ExternalLoadPost] WITH (NOLOCK)
                                    WHERE        IsActive = 1
                                    ORDER BY     ID DESC;";
                using (var multi = connection.QueryMultiple(query))
                {
                    var result = multi.Read<int>();
                    return result.FirstOrDefault();
                }
            }
        }

        public static int GetValidInternalEmployeeIdToRetrieveExternalLoadPostCredential(string connectionString)
        {
            connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));

            using (var connection = new SqlConnection(connectionString))
            {
                string query = $@"	SELECT  TOP 1 [InternalEmployeeId] 
                            		FROM    [dbo].[ExternalLoadPostCredential] WITH (NOLOCK);";
                using (var multi = connection.QueryMultiple(query))
                {
                    var result = multi.Read<int>();
                    return result.FirstOrDefault();
                }
            }
        }

        public static void RemoveExternalLoadPost(int newExternalLoadPostId, int oldExternalLoadPostId, string connectionString)
        {
            connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));

            var param = new DynamicParameters();
            param.Add("@NewExternalLoadPostId", newExternalLoadPostId, dbType: DbType.Int32);
            param.Add("@OldExternalLoadPostId", oldExternalLoadPostId, dbType: DbType.Int32);

            using (var connection = new SqlConnection(connectionString))
            {
                string query = $@"	DELETE
                                    FROM    [dbo].[ExternalLoadPost]
                                    WHERE   Id = @NewExternalLoadPostId;

                                    UPDATE  [dbo].[ExternalLoadPost] 
                                    SET     IsActive = 1
                                    WHERE   Id = @OldExternalLoadPostId;";

                connection.ExecuteScalar(query,param: param);
            }
        }
    }
}