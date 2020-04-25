using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.Products.Api.Models;

namespace Xero.Products.Api.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(Guid id);
        Task Create(Product product);
        Task Delete(Guid id);
        Task Update(Product original);
    }
}
