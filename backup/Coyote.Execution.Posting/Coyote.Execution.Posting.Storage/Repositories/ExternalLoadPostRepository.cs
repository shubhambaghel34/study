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
    using Coyote.Execution.Posting.Contracts.Commands;
    using Coyote.Execution.Posting.Contracts.Models;
    using Coyote.Execution.Posting.Contracts.Storage;
    using Dapper;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExternalLoadPostRepository : IExternalLoadPostRepository
    {
        #region " Private properties "
        private readonly string _connectionString;
        #endregion

        #region " Constructor "
        public ExternalLoadPostRepository(string connectionString)
        {
            _connectionString = connectionString.ThrowIfArgumentNullOrEmpty(nameof(connectionString));
        }
        #endregion

        #region " Public Methods "
        public async Task<LoadPost> GetActivePostDetailsByLoadId(int loadId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@LoadId", loadId);

                string query = $@"SELECT	[ID],
				                            [LoadId],
				                            [OriginCityId],
				                            [DestinationCityId],
				                            [PickupDate],
				                            [PickupDateUTC],
				                            [IsLoadPartial],
				                            [EquipmentType],
				                            [EquipmentLength],
				                            [Weight],
				                            [Rate],
				                            [NumberOfStops],
				                            [Notes],
				                            [IsActive],
				                            [ITSPostStatus],
				                            [DATPostStatus],
				                            [PostEverywherePostStatus],
				                            [PostedAsUserId],
				                            [CreateByUserID],
				                            [UpdateByUserID],
				                            [CreateDate],
				                            [UpdateDate],
				                            [ExternalLoadPostActionId],
				                            [IsPostedWhenCovered]
		                            FROM	[dbo].[ExternalLoadPost] (NOLOCK)
		                            WHERE	[IsActive] = 1 AND [LoadId] = {loadId};";

                using (var multi = await connection.QueryMultipleAsync(query, param: dynamicParameters, commandType: CommandType.Text))
                {
                    var result = await multi.ReadAsync<LoadPost>();
                    return result.FirstOrDefault();
                }
            }
        }

        public async Task<ExternalLoadPostCredential> GetExternalLoadPostCredentialByInternalEmployeeId(int internalEmployeeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();
                param.Add("InternalEmployeeId", internalEmployeeId, dbType: DbType.Int32);
                using (var multi = await connection.QueryMultipleAsync("spExternalLoadPostCredential_GetDetailsByInternalEmployeeId", param, commandType: CommandType.StoredProcedure))
                {
                    var result = await multi.ReadAsync<ExternalLoadPostCredential>();
                    return result.FirstOrDefault();
                }
            }
        }

        public async Task<int> InsertAndUpdateExternalLoadPost(LoadPostBase loadPostBase)
        {
            loadPostBase.ThrowIfArgumentNull(nameof(loadPostBase));

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@LoadId", loadPostBase.LoadId);
                param.Add("@OriginCityId", loadPostBase.Origin.Id);
                param.Add("@DestinationCityId", loadPostBase.Destination.Id);
                param.Add("@PickupDate", loadPostBase.PickUpDate);
                param.Add("@PickupDateUTC", loadPostBase.PickUpDate);
                param.Add("@IsLoadPartial", loadPostBase.IsLoadPartial);
                param.Add("@EquipmentType", loadPostBase.EquipmentType);
                param.Add("@EquipmentLength", loadPostBase.EquipmentLength);
                param.Add("@Weight", loadPostBase.Weight);
                param.Add("@Rate", loadPostBase.Rate);
                param.Add("@NumberOfStops", loadPostBase.NumberOfStops);
                param.Add("@Notes", loadPostBase.Notes);
                param.Add("@IsActive", true);
                param.Add("@ITSPostStatus", loadPostBase.ITSPostStatus);
                param.Add("@DATPostStatus", loadPostBase.DATPostStatus);
                param.Add("@PostEverywherePostStatus", loadPostBase.PostEverywherePostStatus);
                param.Add("@PostedAsUserId", loadPostBase.PostedAsUserId);
                param.Add("@UserId", loadPostBase.UserId);
                param.Add("@ExternalLoadPostActionId", loadPostBase.ExternalLoadPostActionId);
                param.Add("@IsPostedWhenCovered", loadPostBase.IsPostedWhenCovered);

                return await connection.ExecuteAsync("[dbo].[spExternalLoadPost_InsertAndUpdate]", param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<LoadPost> GetActivePostDetailsByLoaIdForAutoRefresh(int loadId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@LoadId", loadId);

                string query = $@"SELECT    EP.[LoadId],
                                            EP.[DestinationCityId],
                                            EP.[OriginCityId],
											LB.[Team],
											LB.[Hazmat],
                                            EP.[EquipmentType],
                                            EP.[EquipmentLength],
				                            EP.[PickupDate],
				                            EP.[IsLoadPartial],
				                            EP.[Weight],
				                            EP.[Rate],
											EP.[NumberOfStops],
				                            EP.[Notes],
				                            EP.[ITSPostStatus],
				                            EP.[DATPostStatus],
				                            EP.[PostEverywherePostStatus],
				                            EP.[PostedAsUserId],
                                            LB.[ProgressType],
                                            LB.[StateType]
		                            FROM	[dbo].[ExternalLoadPost] EP (NOLOCK)
											JOIN
											[dbo].[LoadBoard] LB (NOLOCK)
											ON LB.LoadID = EP.LoadId
		                            WHERE	EP.[IsActive] = 1 AND LB.[LoadId] = @LoadId;";

                using (var multi = await connection.QueryMultipleAsync(query, param: dynamicParameters, commandType: CommandType.Text))
                {
                    var result = await multi.ReadAsync<LoadPost>();
                    return result.FirstOrDefault();
                }
            }
        }
        #endregion
    }
}