namespace Demo.Storage.Dapper.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Demo.Common.Enums;
    using Demo.Common.Extensions;
    using Demo.Contract.Interfaces.Storage.Repositories;
    using Demo.Contract.Models;
    using global::Dapper;

    public class UserRepository : IUserRepository
    {
        #region " Private properties "
        private readonly string _connectionString;
        private readonly IAddressRepository _addressRepository;
        #endregion

        #region " Constructor "
        public UserRepository(string ConnectionString, IAddressRepository AddressRepository)
        {
            _connectionString = ConnectionString.ThrowIfArgumentNullOrEmpty(nameof(ConnectionString));
            _addressRepository = AddressRepository.ThrowIfArgumentNull(nameof(AddressRepository));
        }
        #endregion

        #region " Public Methods "
        public async Task<int> AddUserAsync(User User)
        {
            User.ThrowIfArgumentNull(nameof(User));

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@Code", User.Code);
                param.Add("@FirstName", User.FirstName);
                param.Add("@LastName", User.LastName);
                param.Add("@Email", User.Email);
                param.Add("@Mobile", User.Mobile);

                var result = await connection.QuerySingleOrDefaultAsync<int>("[dbo].[spUser_Insert]", param, commandType: CommandType.StoredProcedure);

                if (result > 0 && User.Address != null)
                {
                    User.Address.EntityId = result;
                    User.Address.EntityType = (int)EntityType.User;

                    await _addressRepository.AddAddressAsync(User.Address);
                }

                return result;
            }
        }

        public async Task<IEnumerable<User>> GetALLUsersAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var multi = await connection.QueryMultipleAsync("spUser_GetAll", commandType: CommandType.StoredProcedure))
                {
                    var result = await multi.ReadAsync<User>();

                    foreach (var user in result)
                    {
                        user.Address = await _addressRepository.GetAddressByTypeAndIdAsync((int)EntityType.User, user.Id);
                        if (user.Address != null)
                        {
                            user.Address.EntityId = user.Id;
                            user.Address.EntityType = (int)EntityType.User;
                        }                       
                    }
                    return result;
                }
            }
        }

        public async Task<User> GetUserByCodeAsync(string Code)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Code", Code);

                using (var multi = await connection.QueryMultipleAsync("spUser_GetByCode", param: dynamicParameters, commandType: CommandType.StoredProcedure))
                {
                    var result = await multi.ReadAsync<User>();
                    var user = result.FirstOrDefault();
                    if (user != null)
                    {
                        user.Address = await _addressRepository.GetAddressByTypeAndIdAsync((int)EntityType.User, user.Id);
                        if (user.Address != null)
                        {
                            user.Address.EntityId = user.Id;
                            user.Address.EntityType = (int)EntityType.User;
                        }
                    }

                    return user;
                }
            }
        }
        #endregion
    }
}
