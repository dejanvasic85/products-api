using Microsoft.Data.Sqlite;
using System;
using System.Data;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Repository
{
    public class ConnectionFactory : IConnectionFactory, IDisposable
    {
        private readonly IAppConfig _config;

        private IDbConnection _dbConnection;

        public ConnectionFactory(IAppConfig config)
        {
            _config = config;
        }

        public IDbConnection CreateConnection()
        {
            if (_dbConnection != null)
            {
                return _dbConnection;
            }

            _dbConnection = new SqliteConnection(_config.DatabaseConfig.ConnectionString);

            return _dbConnection;
        }

        public void Dispose()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Dispose();
            }

        }
    }
}
