using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xero.Products.BusinessLayer
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ProductService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<IEnumerable<Product>> GetProducts(string name = "")
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                var products = string.IsNullOrEmpty(name)
                    ? await unitOfWork.ProductRepository.GetAll()
                    : await unitOfWork.ProductRepository.GetProductsByName(name);

                return products;
            }
        }

        public async Task<Product> GetProductById(Guid id)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                return await unitOfWork.ProductRepository.GetById(id);
            }
        }

        public async Task<Product> CreateProduct(Product product)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                var newProduct = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = product.Name,
                    Description = product.Description,
                    DeliveryPrice = product.DeliveryPrice,
                    Price = product.Price
                };

                await unitOfWork.ProductRepository.Create(newProduct);
                unitOfWork.Commit();

                return newProduct;
            }
        }

        public async Task UpdateProduct(Product productToUpdate)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                var originalProduct = await unitOfWork.ProductRepository.GetById(productToUpdate.Id);
                if (originalProduct == null)
                {
                    throw new NullReferenceException($"The product to update does not exist. ProductId: {productToUpdate.Id}");
                }

                originalProduct.Name = productToUpdate.Name;
                originalProduct.Description = productToUpdate.Description;
                originalProduct.Price = productToUpdate.Price;
                originalProduct.DeliveryPrice = productToUpdate.DeliveryPrice;

                await unitOfWork.ProductRepository.Update(originalProduct);
            }
        }

        public async Task DeleteProduct(Guid id)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                await unitOfWork.ProductRepository.Delete(id);
                await unitOfWork.ProductOptionRepository.DeleteByProductId(id);
                unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                return await unitOfWork.ProductOptionRepository.GetAll(productId);
            }
        }

        public async Task<ProductOption> GetProductOption(Guid productId, Guid id)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                return await unitOfWork.ProductOptionRepository.GetById(productId, id);
            }
        }

        public async Task<ProductOption> CreateProductOption(Guid productId, ProductOption productOption)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                var newOption = new ProductOption
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    Name = productOption.Name,
                    Description = productOption.Description,
                };

                await unitOfWork.ProductOptionRepository.Create(newOption);
                unitOfWork.Commit();

                return newOption;
            }
        }

        public async Task UpdateProductOption(Guid productId, ProductOption productOption)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                var originalOption = await unitOfWork.ProductOptionRepository.GetById(productId, productOption.Id);
                if (originalOption == null)
                {
                    throw new NullReferenceException($"Unable to retrieve product option by id {productOption.Id} and productId {productId}");
                }

                originalOption.Name = productOption.Name;
                originalOption.Description = productOption.Description;

                await unitOfWork.ProductOptionRepository.Update(originalOption);
                unitOfWork.Commit();
            }
        }

        public async Task DeleteProductOption(Guid id)
        {
            using (var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork())
            {
                await unitOfWork.ProductOptionRepository.Delete(id);
                unitOfWork.Commit();
            }
        }
    }

}
