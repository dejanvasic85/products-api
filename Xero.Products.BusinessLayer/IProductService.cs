using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Xero.Products.BusinessLayer
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts(string name = "");
        Task<Product> GetProductById(Guid id);
        Task<Product> CreateProduct(Product product);
        Task UpdateProduct(Product productToUpdate);
        Task DeleteProduct(Guid id);

        Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId);
        Task<ProductOption> GetProductOption(Guid productId, Guid id);
        Task<ProductOption> CreateProductOption(ProductOption productOption);
        Task UpdateProductOption(Guid productId, ProductOption productOption);
        Task DeleteProductOption(Guid id);
    }

}
