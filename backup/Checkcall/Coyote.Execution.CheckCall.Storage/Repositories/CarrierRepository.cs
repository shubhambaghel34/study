// /////////////////////////////////////////////////////////////////////////////////////
//                           Copyright (c) 2016 - 2018
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
    using Coyote.Common.Extensions;
    using Coyote.Execution.CheckCall.Domain.Models;
    using Dapper;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class CarrierRepository : ICarrierRepository
    {
        #region  Private properties
        private readonly string _connectionString;
        private ILog Log { get; set; }
        #endregion

        #region  Constructor
        public CarrierRepository(string connectionString, ILog log)
        {
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
            Log = log.ThrowIfNull(nameof(log));
        }
        #endregion

        #region  Public Methods
        public async Task<Carrier> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                Carrier result = null;
                string sql = $"SELECT Id,Name,ContractVersion FROM dbo.Carrier WITH (NOLOCK) " +
                             $"WHERE Id=@id;" +
                             $"SELECT R.Id,R.RepType,EMP.Id,R.Main,EMP.EmailWork,EMP.FirstName, " +
                             $"EMP.LastName,EMP.OfficePhoneNumber,EMP.MobilePhoneNumber,EMP.FaxNumber  " +
                             $"FROM dbo.Rep R WITH (NOLOCK) INNER JOIN  dbo.Employee EMP WITH (NOLOCK) " +
                             $"ON R.EmployeeId=EMP.Id " +
                             $"WHERE EntityId=@id AND EntityType = {(int)EntityType.Carrier}";

                using (var multi = await connection.QueryMultipleAsync(sql: sql, param: new { Id = id }, commandType: CommandType.Text))
                {
                    result = multi.Read<Carrier>().FirstOrDefault();
                    if (result != null)
                    {
                        result.Reps = multi.Read<Rep>().ToList();
                        if (result.Reps != null)
                        {
                            foreach (var rep in result.Reps)
                            {
                                rep.Addresses = GetRepAddressDetails(rep.Id).Result;
                            }
                        }
                    }
                }
                return result;
            }
        }

        private async Task<ICollection<Address>> GetRepAddressDetails(int repId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<Address>(sql:
                            $"SELECT Address1,Address2,ZipCode,LC.Name AS CityName " +
                            $"FROM dbo.Address A WITH (NOLOCK) INNER JOIN dbo.LocationCity LC WITH (NOLOCK) " +
                            $"ON A.CityId = LC.CityId " +
                            $"WHERE EntityId=@repId AND EntityType={(int)EntityType.Employee}"
                            , param: new { RepId = repId }
                            , commandType: CommandType.Text);

                return result.ToList();
            }
        }

        public async Task<List<CheckCallNotificationRecord>> GetCarrierCheckCallNotificationRecords(bool isLoadDateToday)
        {
            DateTime midnightLastNight = DateTime.Now.Date;
            DateTime midnightTonight = DateTime.Now.Date.AddDays(1);

            if (!isLoadDateToday)
            {
                midnightLastNight = DateTime.Now.Date.AddDays(1);
                midnightTonight = DateTime.Now.Date.AddDays(2);
            }
            Log.InfoFormat("Query DB for Check Call Notifications between {0} and {1}", midnightLastNight.ToString("u"), midnightTonight.ToString("u"));

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("@midnightLastNight", midnightLastNight, DbType.DateTime);
                param.Add("@midnightTonight", midnightTonight, DbType.DateTime);
                param.Add("@loadModeType", LoadModeType.TL, DbType.Int32);
                param.Add("@loadStateType", LoadStateType.Active, DbType.Int32);
                param.Add("@loadProgressType", LoadProgressType.Covered, DbType.Int32);
                var result = await connection.QueryAsync<CheckCallNotificationRecord>
                    (sql: $"SELECT CTP.CarrierId,Email,EmailName,L.Id AS LoadId, " +
                          $"L.LoadDate,L.OriginCityName,L.OriginStateCode as OriginState,L.DestinationCityName as DestCityName,L.DestinationStateCode as DestState,LS.ScheduleOpenTime,LS.ScheduleCloseTime " +
                          $"FROM [dbo].CARRIERTRACKINGPREFERENCE CTP WITH (NOLOCK) " +
                          $"INNER JOIN [dbo].LOADCarrier LC WITH (NOLOCK) ON CTP.CARRIERID = LC.CARRIERID AND LC.Main = 1 " +
                          $"INNER JOIN [dbo].Load L WITH (NOLOCK) ON LC.LoadId = L.Id " +
                          $"INNER JOIN [dbo].LoadStop LS WITH (NOLOCK) ON LS.LoadId = L.Id AND LS.Sequence = 1 " +
                          $"WHERE CTP.DailyCheckCallEmail = 1 AND " +
                          $"CTP.Email IS NOT NULL AND " +
                          $"CTP.Email <> '' AND " +
                          $"L.Mode = @loadModeType AND " +
                          $"L.StateType = @loadStateType AND " +
                          $"L.ProgressType = @loadProgressType AND " +
                          $"L.EquipmentType NOT IN ('P') AND " +
                          $"L.LoadDate >= @midnightLastNight AND " +
                          $"L.LoadDate < @midnightTonight " +
                          $"ORDER BY CTP.CarrierId"
                          , param: param
                          , commandType: CommandType.Text);
                return result.ToList();
            }
        }

        #endregion
    }
}
