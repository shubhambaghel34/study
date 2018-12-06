namespace Demo.Storage.Dapper
{
    using Demo.Common.Extensions;
    using global::Dapper;
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class DatabaseConnectionTest
    {
        public string _connectionString { get; set; }
        public DatabaseConnectionTest(string ConnectionString)
        {
            _connectionString = ConnectionString.ThrowIfArgumentNullOrEmpty(nameof(ConnectionString));
        }

        public async Task<bool> TryConnect()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                    await connection.ExecuteAsync("SELECT 1");
                    return true ;
            }
        }
    }
}
