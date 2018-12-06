namespace Demo.Storage.Dapper.Repositories
{
    using Demo.Common.Extensions;
    using Demo.Contract.Interfaces.Storage.Repositories;
    using Demo.Contract.Models;
    using global::Dapper;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductRepository : IProductRepository
    {
        #region " Private properties "
        private readonly string _connectionString;
        #endregion

        #region " Constructor "
        public ProductRepository(string ConnectionString)
        {
            _connectionString = ConnectionString.ThrowIfArgumentNullOrEmpty(nameof(ConnectionString));
        }
        #endregion

        #region " Public Methods "
        public async Task<Product> GetProductByCodeAsync(string code)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Code", code);

                using (var multi = await connection.QueryMultipleAsync("spProduct_GetByCode", param: dynamicParameters, commandType: CommandType.StoredProcedure))
                {
                    var result = await multi.ReadAsync<Product>();
                    return result.FirstOrDefault();
                }
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var multi = await connection.QueryMultipleAsync("spProducts_GetAll", commandType: CommandType.StoredProcedure))
                {
                    var result = await multi.ReadAsync<Product>();
                    return result;
                }
            }
        }

        public async Task<Product> GetProductByIdAsync(int Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@Id", Id);

                using (var multi = await connection.QueryMultipleAsync("spProduct_GetById", param: dynamicParameters, commandType: CommandType.StoredProcedure))
                {
                    var result = await multi.ReadAsync<Product>();
                    return result.FirstOrDefault();
                }
            }
        }

        public async Task<int> AddProductAsync(Product Product)
        {
            Product.ThrowIfArgumentNull(nameof(Product));

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@Code", Product.Code);
                param.Add("@Name", Product.Name);
                param.Add("@ReleaseDate", Product.ReleaseDate);
                param.Add("@Price", Product.Price);
                param.Add("@Description", Product.Description);
                param.Add("@ImageUrl", Product.ImageUrl);
                param.Add("@StarRating", Product.StarRating);

                var result = await connection.QuerySingleOrDefaultAsync<int>("[dbo].[spProduct_Insert]", param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<int> UpdateProductByCodeAsync(Product Product)
        {
            Product.ThrowIfArgumentNull(nameof(Product));

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@Name", Product.Name);
                param.Add("@Code", Product.Code);
                param.Add("@ReleaseDate", Product.ReleaseDate);
                param.Add("@Price", Product.Price);
                param.Add("@Description", Product.Description);
                param.Add("@ImageUrl", Product.ImageUrl);
                param.Add("@StarRating", Product.StarRating);

                var result = await connection.ExecuteAsync("[dbo].[spProduct_UpdateByCode]", param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<int> DeleteProductByCodeAsync(string Code)
        {
            Code.ThrowIfArgumentNullOrEmpty(nameof(Code));

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@Code", Code);

                var result = await connection.ExecuteAsync("[dbo].[spProduct_DeleteByCode]", param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
        public async Task<int> UpdateProductByIdAsync(Product Product)
        {
            Product.ThrowIfArgumentNull(nameof(Product));

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@ProductId", Product.ProductId);
                param.Add("@Name", Product.Name);
                param.Add("@Code", Product.Code);
                param.Add("@ReleaseDate", Product.ReleaseDate);
                param.Add("@Price", Product.Price);
                param.Add("@Description", Product.Description);
                param.Add("@ImageUrl", Product.ImageUrl);
                param.Add("@StarRating", Product.StarRating);

                var result = await connection.ExecuteAsync("[dbo].[spProduct_UpdateById]", param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }

        public async Task<int> DeleteProductByIdAsync(int Id)
        {
            Id.ThrowIfArgumentEqualTo(nameof(Id), 0);

            using (var connection = new SqlConnection(_connectionString))
            {
                var param = new DynamicParameters();

                param.Add("@ProductId", Id);

                var result = await connection.ExecuteAsync("[dbo].[spProduct_DeleteById]", param, commandType: CommandType.StoredProcedure);

                return result;
            }
        }
        #endregion
    }
}
