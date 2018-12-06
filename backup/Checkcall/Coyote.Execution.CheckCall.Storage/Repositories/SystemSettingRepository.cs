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
namespace Coyote.Execution.CheckCall.Storage.Repositories
{
    using Coyote.Execution.CheckCall.Domain.Models;
    using Dapper;
    using System.Data;
    using System.Data.SqlClient;    
    using System.Threading.Tasks;

    public class SystemSettingRepository : ISystemSettingRepository
    {
        #region  Private properties
        private readonly string _connectionString;
        #endregion

        #region  Constructor 
        public SystemSettingRepository(string connectionString)
        {
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
        }
        #endregion

        public async Task<SystemSettings> GetBySettingName(string settingName)
        {            
            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@settingName", settingName, DbType.String);
                var result = await connection.QueryFirstOrDefaultAsync<SystemSettings>(sql:
                            $"SELECT SettingName,SettingValue " +
                            $"FROM dbo.SystemSettings WITH (NOLOCK) " +
                            $"WHERE SettingName=@settingName"
                            , param: param
                            , commandType: CommandType.Text);
                return result;
            }
        }
    }
}