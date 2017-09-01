using System;
using System.Configuration;
using System.Data.SqlClient;

namespace Example.Infrastructure.Data.DapperConfig
{
    public class DapperContext : IDisposable
    {
        private readonly string _connectionString;

        public DapperContext()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["EntityContext"].ConnectionString;
        }

        public SqlConnection SqlConnection()
        {
            return new SqlConnection(_connectionString);
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            SqlConnection().Close();
        }
    }
}
