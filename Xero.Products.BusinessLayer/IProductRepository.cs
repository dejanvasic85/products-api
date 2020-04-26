using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xero.Products.BusinessLayer
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetProductsByName(string name);
        Task<Product> GetById(Guid id);
        Task Create(Product product);
        Task Delete(Guid id);
        Task Update(Product original);
    }
}
