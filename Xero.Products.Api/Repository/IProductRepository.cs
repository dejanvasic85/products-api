using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.Products.Api.Models;

namespace Xero.Products.Api.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(Guid id);
        Task CreateProduct(Product product);
        Task DeleteProduct(Guid id);
    }
}
