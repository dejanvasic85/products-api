using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xero.Products.BusinessLayer
{
    public interface IProductOptionRepository
    {
        Task<IEnumerable<ProductOption>> GetAll(Guid productId);
        Task<ProductOption> GetById(Guid productId, Guid id);
        Task Create(ProductOption option);
        Task Update(ProductOption option);
        Task Delete(Guid id);
        Task DeleteByProductId(Guid productId);
    }
}
