using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Xero.Products.Api.Models;

namespace Xero.Products.Api.Repository
{
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
                return await db.QueryAsync<ProductOption>("select * from ProductOptions where productid = @ProductId collate nocase", new
                {
                    ProductId = productId
                });
            }
        }

        public async Task<ProductOption> GetProductOption(Guid productId, Guid id)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                return await db.QuerySingleOrDefaultAsync<ProductOption>("select * from ProductOptions where productid = @ProductId and id = @Id collate nocase", new
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
                string insertQuery = $"insert into ProductOptions (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)";

                await db.ExecuteAsync(insertQuery, option);
            }
        }

        public async Task UpdateProductOption(ProductOption option)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                var updateQuery = "update ProductOptions set name = @Name, description = @Description where id = @Id collate nocase";
                await db.ExecuteAsync(updateQuery, option);
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                await db.ExecuteAsync("delete from ProductOptions where Id = @Id collate nocase", new
                {
                    Id = id
                });
            }
        }
    }
}
