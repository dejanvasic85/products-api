using System.Data;

namespace Xero.Products.BusinessLayer
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
