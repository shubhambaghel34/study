using Demo.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Storage.EF
{
    public class DatabaseConnectionTest
    {
        public string _connectionString { get; set; }
        public DatabaseConnectionTest(string ConnectionString)
        {
            _connectionString = ConnectionString.ThrowIfArgumentNullOrEmpty(nameof(ConnectionString));
        }

        public async Task<bool> TryConnect()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    //await connection.
                    return true;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
