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
        Task<ProductOption> GetProductOption(Guid productId, Guid id);
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

        public async Task<ProductOption> GetProductOption(Guid productId, Guid id)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                return await db.QuerySingleOrDefaultAsync<ProductOption>("select * from productoptions where productid = @ProductId and id = @Id collate nocase", new
                {
                    ProductId = productId,
                    Id = id
                });
            }
        }
    }
}
