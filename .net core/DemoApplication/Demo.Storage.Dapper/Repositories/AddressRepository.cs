
namespace Demo.Storage.Dapper.Repositories
{
    using Demo.Common.Extensions;
    using Demo.Contract.Interfaces.Storage.Repositories;
    using Demo.Contract.Models;
    using global::Dapper;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class AddressRepository : IAddressRepository
    {
        #region " Private properties "
        private readonly string _connectionString;
        #endregion

        #region " Constructor "
        public AddressRepository(string ConnectionString)
        {
            _connectionString = ConnectionString.ThrowIfArgumentNullOrEmpty(nameof(ConnectionString));
        }
        #endregion

        #region " Public Methods "
        public async Task<int> AddAddressAsync(Address Address)
        {
            Address.ThrowIfArgumentNull(nameof(Address));

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@EntityId", Address.EntityId);
                param.Add("@EntityType", Address.EntityType);
                param.Add("@City", Address.City);
                param.Add("@State", Address.State);
                param.Add("@Country", Address.Country);
                param.Add("@AddressLine", Address.AddressLine);
                param.Add("@Street", Address.Street);
                param.Add("@AddressType", Address.AddressType);

                var result = await connection.QuerySingleOrDefaultAsync<int>("[dbo].[spAddress_Insert]", param, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<Address> GetAddressByTypeAndIdAsync(int EntityType, int EntityId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@EntityType", EntityType);
                dynamicParameters.Add("@EntityId", EntityId);

                using (var multi = await connection.QueryMultipleAsync("[dbo].[spAddress_GetByTypeAndId]", param: dynamicParameters, commandType: CommandType.StoredProcedure))
                {
                    var result = await multi.ReadAsync<Address>();
                    return result.FirstOrDefault();
                }
            }
        }
        #endregion
    }
}
