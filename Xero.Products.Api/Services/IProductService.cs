using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.Products.Api.Models;

namespace Xero.Products.Api.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
