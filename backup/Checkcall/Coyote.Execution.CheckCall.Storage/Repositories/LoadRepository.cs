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
namespace Coyote.Execution.CheckCall.Storage.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Coyote.Execution.CheckCall.Domain.Models;
    using Dapper;

    public class LoadRepository : ILoadRepository
    {
        #region  Private properties 
        private readonly string _connectionString;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        #endregion

        #region  Constructor 
        public LoadRepository(string connectionString)
        {
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
        }
        #endregion

        #region Public Methods
        public async Task<Load> GetById(int loadId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                Load result = null;
                string sql = $"SELECT L.Id, L.LoadDate, L.Mode, L.StateType, L.ProgressType, " +
                             $"L.EquipmentType, L.OriginCityName, L.OriginStateCode, " +
                             $"L.DestinationCityName, L.DestinationStateCode, L.Division," +
                             $"LB.OriginAlpha2CountryCode AS OriginCountryCode, LB.DestAlpha2CountryCode AS DestinationCountryCode " +
                             $"FROM dbo.Load L WITH (NOLOCK) " +
                             $"INNER JOIN dbo.LoadBoard LB WITH (NOLOCK) ON LB.LoadID = L.Id " +
                             $"WHERE L.Id = @loadId;" +
                             $"SELECT Id, CarrierId " +
                             $"FROM dbo.LoadCarrier WITH (NOLOCK) " +
                             $"WHERE LoadId = @loadId AND Main = 1;" +
                             $"SELECT Id, Name " +
                             $"FROM dbo.LoadCustomer WITH (NOLOCK) " +
                             $"WHERE LoadId = @loadId AND Main = 1;";

                using (var multi = await connection.QueryMultipleAsync(sql: sql, param: new { LoadId = loadId }, commandType: CommandType.Text))
                {
                    result = multi.Read<Load>().FirstOrDefault();
                    if (result != null)
                    {
                        result.MainLoadCarrier = multi.Read<LoadCarrier>().FirstOrDefault();
                        if (result.MainLoadCarrier != null)
                        {
                            result.MainLoadCarrier.LoadReps = GetLoadCarrierReps(result.MainLoadCarrier.Id).Result;
                        }
                        result.MainLoadCustomer = multi.Read<LoadCustomer>().FirstOrDefault();
                    }
                }
                return result;
            }
        }



        public async Task<ICollection<LoadRep>> GetLoadCarrierReps(int loadCarrierId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {                
                var result = await connection.QueryAsync<LoadRep>(sql:
                            $"SELECT LR.Id, EMP.Id AS EmployeeId, EMP.EmailWork " +
                            $"FROM dbo.LoadRep LR WITH (NOLOCK) INNER JOIN dbo.Employee EMP WITH (NOLOCK) " +
                            $"ON LR.EmployeeId = EMP.Id " +
                            $"WHERE EntityId=@loadCarrierId AND EntityType={(int)EntityType.LoadCarrier} "
                            , param: new { LoadCarrierId = loadCarrierId }
                            , commandType: System.Data.CommandType.Text);

                return result.ToList();
            }

        }

        #endregion
    }
}
