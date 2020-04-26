using Microsoft.Extensions.Options;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Configuration
{
    public class AppConfig : IAppConfig
    {
        private readonly IOptions<DatabaseConfig> _databaseConfig;

        public AppConfig(IOptions<DatabaseConfig> databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public DatabaseConfig DatabaseConfig => _databaseConfig.Value;
    }
}
