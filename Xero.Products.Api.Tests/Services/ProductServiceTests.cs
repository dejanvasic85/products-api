using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Products.Api.Controllers;
using Xero.Products.Api.Models;
using Xero.Products.Api.Repository;

namespace Xero.Products.Api.Tests
{
    public class ProductControllerTests
    {
        private ProductsController _productsController;

        private MockRepository _mockRepository;
        private Mock<IProductRepository> _productRepository;
        private Mock<IProductOptionRepository> _productOptionRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _productRepository = _mockRepository.Create<IProductRepository>();
            _productOptionRepository = _mockRepository.Create<IProductOptionRepository>();

            _productsController = new ProductsController(_productRepository.Object, _productOptionRepository.Object);
        }

        [Test]
        public async Task GetAllProducts_CallsRepository_ReturnsProductList()
        {
            var results = new List<Product>()
            {
                new Product()
                {
                    Name = "Product 1"
                }
            };

            //var options = new List<ProductOption>()
            //{
            //    new ProductOption()
            //    {
            //        Name = "Option 1"
            //    }
            //};

            _productRepository.Setup(call => call.GetAllProducts()).Returns(Task.FromResult(results.AsEnumerable())).Verifiable();
            // _productOptionRepository.Setup(call => call.GetProductOptions(It.IsAny<Guid>())).Returns(Task.FromResult(options.AsEnumerable())).Verifiable();


            var result = await _productsController.Get();

            Assert.That(result, Is.EqualTo(results));

            _mockRepository.VerifyAll();
        }       
    }
}