using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.Products.Api.Configuration;
using Xero.Products.Api.Models;

namespace Xero.Products.Api.Repository
{
    public interface IProductRepository
    {
        Task<IList<Product>> GetAllProducts();
    }

    public class ProductRepository : IProductRepository
    {
        public ProductRepository(IOptions<DatabaseConfig> databaseConfig)
        {
            System.Console.WriteLine($"connString {databaseConfig.Value.ConnectionString}");
        }

        public async Task<IList<Product>> GetAllProducts()
        {
            return await Task.FromResult(new List<Product>
            {
                new Product { Name = "Coca Cola" }
            });
        }
    }
}
