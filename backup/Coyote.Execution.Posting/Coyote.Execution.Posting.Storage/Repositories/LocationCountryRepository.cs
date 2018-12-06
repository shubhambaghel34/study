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
namespace Coyote.Execution.Posting.Storage.Repositories
{
    using Coyote.Execution.Posting.Common.Extensions;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Dapper;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public class LocationCountryRepository : ILocationCountryRepository
    {
        #region " Private properties "
        private readonly string _connectionString;
        #endregion

        #region " Constructor "
        public LocationCountryRepository(string connectionString)
        {
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
        }
        #endregion

        #region " Public Methods "
        public LocationCountry GetLocationCountryByCityId(int cityId)
        {
            LocationCountry result = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@CityID", cityId, dbType: DbType.Int32);

                using (var multi = connection.QueryMultiple("spLocationCountry_GetDetailsByCityId", param, commandType: CommandType.StoredProcedure))
                {
                    result = multi.Read<LocationCountry>().FirstOrDefault();
                }
                return result;
            }
        }

        public City GetCityDetailsByCityId(int cityId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@CityId", cityId);

                string query = $@"SELECT     LC.CityId AS Id,
                                             LC.Name,
                                             LS.Code AS StateCode,
                                             RTRIM(LTRIM(ISNULL(LPC.PostalCode,''))) AS MainZipCode
                                  FROM       dbo.[LocationCity] LC (NOLOCK)
                                  INNER JOIN dbo.[LocationStateCountry] LSC (NOLOCK)
                                  ON         LC.LocationStateCountryId = LSC.LocationStateCountryID
                                  INNER JOIN dbo.[LocationState] LS (NOLOCK)
                                  ON         LS.LocationStateId = LSC.LocationStateId
                                  LEFT JOIN dbo.[LocationPostalCode] LPC (NOLOCK)
                                  ON         LC.CityId = LPC.CityId AND LPC.Main = 1
                                  WHERE      LC.CityId = @CityId;";

                using (var multi = connection.QueryMultiple(query, param: dynamicParameters, commandType: CommandType.Text))
                {
                    return multi.Read<City>().FirstOrDefault();
                }
            }
        }
        #endregion
    }
}