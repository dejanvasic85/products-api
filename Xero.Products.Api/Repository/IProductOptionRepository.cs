using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Xero.Products.Api.Models;

namespace Xero.Products.Api.Repository
{
    public interface IProductOptionRepository
    {
        Task<IEnumerable<ProductOption>> GetAllProductOptions(Guid productId);
    }

    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ProductOptionRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ProductOption>> GetAllProductOptions(Guid productId)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                return await db.QueryAsync<ProductOption>("select * from productoptions where productid = @ProductId collate nocase", new
                {
                    ProductId = productId
                });
            }
        }
    }
}
