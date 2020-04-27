using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Repository
{
    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly IDbConnection _dbConnection;

        public ProductOptionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        private static readonly string SELECT_SQL = "select Id, ProductId, Name, Description from ProductOptions";
        private static readonly string INSERT_SQL = "insert into ProductOptions (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)";
        private static readonly string UPDATE_SQL = "update ProductOptions set name = @Name, description = @Description where id = @Id collate nocase";
        private static readonly string DELETE_SQL = "delete from ProductOptions";

        public async Task<IEnumerable<ProductOption>> GetAll(Guid productId)
        {
            return await _dbConnection.QueryAsync<ProductOption>($"{SELECT_SQL} where productid = @ProductId collate nocase", new
            {
                ProductId = productId
            });
        }

        public async Task<ProductOption> GetById(Guid productId, Guid id)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<ProductOption>($"{SELECT_SQL} where productid = @ProductId and id = @Id collate nocase", new
            {
                ProductId = productId,
                Id = id
            });
        }

        public async Task Create(ProductOption option)
        {

            await _dbConnection.ExecuteAsync(INSERT_SQL, option);
        }

        public async Task Update(ProductOption option)
        {
            await _dbConnection.ExecuteAsync(UPDATE_SQL, option);
        }

        public async Task Delete(Guid id)
        {
            await _dbConnection.ExecuteAsync($"{DELETE_SQL} where Id = @Id collate nocase", new
            {
                Id = id
            });
        }

        public async Task DeleteByProductId(Guid productId)
        {
            await _dbConnection.ExecuteAsync($"{DELETE_SQL} where ProductId = @ProductId collate nocase", new
            {
                ProductId = productId
            });
        }
    }
}
