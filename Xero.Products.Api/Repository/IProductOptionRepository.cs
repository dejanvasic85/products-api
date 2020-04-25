using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.Products.Api.Models;

namespace Xero.Products.Api.Repository
{
    public interface IProductOptionRepository
    {
        Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId);
        Task<ProductOption> GetProductOption(Guid productId, Guid id);
        Task CreateProductOption(ProductOption option);
        Task UpdateProductOption(ProductOption option);
        Task Delete(Guid id);
    }
}
