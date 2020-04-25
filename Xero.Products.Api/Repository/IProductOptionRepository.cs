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
        Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId);
        Task<ProductOption> GetProductOption(Guid productId, Guid id);
        Task CreateProductOption(ProductOption option);
    }

    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ProductOptionRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId)
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

        public async Task CreateProductOption(ProductOption option)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                string insertQuery = $"insert into productoptions (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)";

                await db.ExecuteAsync(insertQuery, option);
            }
        }
    }
}
