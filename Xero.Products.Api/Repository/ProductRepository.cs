using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xero.Products.Api.Models;

namespace Xero.Products.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private IConnectionFactory _connectionFactory;

        public ProductRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task CreateProduct(Product product)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                string insertQuery = @"insert into Products (id, name, description, price, deliveryprice) values (@Id, @Name, @Description, @Price, @DeliveryPrice)";

                await connection.ExecuteAsync(insertQuery, new
                {
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.DeliveryPrice,
                });
            }
        }

        public async Task DeleteProduct(Guid id)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("delete from Products where Id = @Id", new
                {
                    Id = id
                });
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Product>("select * from Products");
            }
        }

        public async Task<Product> GetProductById(Guid id)
        {
            using (IDbConnection connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Product>($"select * from Products where Id = '{id}' collate nocase");
            }
        }
    }
}
