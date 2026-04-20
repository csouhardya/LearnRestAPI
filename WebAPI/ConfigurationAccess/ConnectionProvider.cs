using ApplicationCore.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace WebAPI.ConfigurationAccess
{
    public class ConnectionProvider : IConnectionProvider
    {
        private readonly IConfiguration _configuration;

        public ConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Reads the connection string from configuration each time this method is called.
        /// </summary>
        /// <returns>Task containing the connection string named "DefaultConnection".</returns>
        public async Task<SqlConnection> ConnectAsync()
        {
            // Read from configuration on every call
            var connectionString = _configuration["Database_Connection_String"];

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in configuration.");

            var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();
            return conn;
        }
    }
}
