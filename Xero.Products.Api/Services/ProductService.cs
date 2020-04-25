using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.Products.Api.Models;
using Xero.Products.Api.Repository;

namespace Xero.Products.Api.Services
{
    public interface IProductService
    {
        Task<IList<Product>> GetProducts();
    }

    public class ProductService : IProductService
    {
        private IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<IList<Product>> GetProducts()
        {
            return _productRepository.GetAllProducts();
        }
    }
}
