﻿using Dapper;
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

        public async Task Create(Product product)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                string insertQuery = @"insert into Products (id, name, description, price, deliveryprice) values (@Id, @Name, @Description, @Price, @DeliveryPrice)";

                await db.ExecuteAsync(insertQuery, product);
            }
        }

        public async Task Delete(Guid id)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                await db.ExecuteAsync("delete from Products where Id = @Id collate nocase", new
                {
                    Id = id
                });
            }
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                return await db.QueryAsync<Product>("select * from Products");
            }
        }

        public async Task<Product> GetById(Guid id)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                return await db.QueryFirstOrDefaultAsync<Product>($"select * from Products where Id = '{id}' collate nocase");
            }
        }

        public async Task Update(Product product)
        {
            using (IDbConnection db = _connectionFactory.CreateConnection())
            {
                var updateQuery = "update Products set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @Id collate nocase";
                await db.ExecuteAsync(updateQuery, product);
            }
        }
    }
}