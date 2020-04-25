using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using Xero.Products.Api.Configuration;

namespace Xero.Products.Api.Repository
{
    public class ConnectionFactory : IConnectionFactory, IDisposable
    {
        private readonly IOptions<DatabaseConfig> _config;

        private IDbConnection _dbConnection;

        public ConnectionFactory(IOptions<DatabaseConfig> config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            if (_dbConnection != null)
            {
                return _dbConnection;
            }

            _dbConnection = new SqliteConnection(_config.Value.ConnectionString);

            return _dbConnection;
        }

        public void Dispose()
        {
            if(_dbConnection != null)
            {
                _dbConnection.Dispose();
            }
          
        }
    }
}
