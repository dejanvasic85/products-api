using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private IConnectionFactory _connectionFactory;

        public ProductRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private static string SELECT_SQL = "select Id, Name, Description, Price, Deliveryprice from Products";
        private static string INSERT_SQL = "insert into Products (id, name, description, price, deliveryprice) values (@Id, @Name, @Description, @Price, @DeliveryPrice)";
        private static string UPDATE_SQL = "update Products set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @Id collate nocase";
        private static string DELETE_SQL = "delete from Products";

        public async Task<IEnumerable<Product>> GetAll()
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                return await db.QueryAsync<Product>(SELECT_SQL);
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                return await db.QueryAsync<Product>($"{SELECT_SQL} where Name like @Name", new
                {
                    Name = $"%{name}%"
                });
            }
        }

        public async Task<Product> GetById(Guid id)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                return await db.QueryFirstOrDefaultAsync<Product>($"{SELECT_SQL} where Id = @Id collate nocase", new
                {
                    Id = id
                });
            }
        }

        public async Task Create(Product product)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                await db.ExecuteAsync(INSERT_SQL, product);
            }
        }

        public async Task Update(Product product)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                await db.ExecuteAsync(UPDATE_SQL, product);
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                await db.ExecuteAsync($"{DELETE_SQL} where Id = @Id collate nocase", new
                {
                    Id = id
                });
            }
        }
    }
}
