using System.Data;

namespace Xero.Products.Api.Repository
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
