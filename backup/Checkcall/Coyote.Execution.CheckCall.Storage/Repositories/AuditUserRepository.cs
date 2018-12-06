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


namespace Coyote.Execution.CheckCall.Storage.Repositories
{
    using System.Threading.Tasks;
    using System.Data.SqlClient;
    using Dapper;
    public class AuditUserRepository : IAuditUserRepository
    {
        #region  Private properties 
        private readonly string _connectionString;
        #endregion

        #region  Constructor 
        public AuditUserRepository(string connectionString)
        {
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
        }
        #endregion

        #region Public Methods
        public async Task<int> GetAuditUserId(string code)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryFirstOrDefaultAsync<int>(sql:
                             $"SELECT UserId FROM dbo.SystemUser WITH (NOLOCK) " +
                             $"WHERE Code=@code"
                             , param: new { Code = code }
                             , commandType: System.Data.CommandType.Text);
                return result;
            }
        }
        #endregion
    }
}
