using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Tests.Services
{
    public class ProductServiceTests
    {
        private ProductService _productService;

        private Mock<IUnitOfWorkFactory> _unitOfWorkFactory;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IProductRepository> _productRepository;
        private Mock<IProductOptionRepository> _productOptionRepository;

        private MockRepository _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _unitOfWorkFactory = _mockRepository.Create<IUnitOfWorkFactory>();
            _unitOfWork = _mockRepository.Create<IUnitOfWork>();
            _productRepository = _mockRepository.Create<IProductRepository>();
            _productOptionRepository = _mockRepository.Create<IProductOptionRepository>();
            _unitOfWorkFactory.Setup(call => call.CreateUnitOfWork()).Returns(_unitOfWork.Object);
            _unitOfWork.Setup(call => call.Dispose()).Verifiable();

            _productService = new ProductService(_unitOfWorkFactory.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task GetProducts_ByName_CallsRepository_RetunsProducts()
        {
            var query = "samsung";

            var products = new List<Product>()
            {
                new Product { Name = "galaxy" },
                new Product { Name = "milky way" },
            };

            _productRepository
                .Setup(call => call.GetProductsByName(It.Is<string>(name => name == query)))
                .Returns(Task.FromResult(products.AsEnumerable()))
                .Verifiable();

            _unitOfWork
                .Setup(prop => prop.ProductRepository)
                .Returns(_productRepository.Object);

            var results = await _productService.GetProducts(query);

            Assert.That(results, Is.EqualTo(products));
        }

        [Test]
        public async Task CreateProduct_CallsRepositorySuccessfully_ReturnsProduct()
        {
            var product = new Product
            {
                Name = "foo",
                Description = "bar",
                Price = 10,
                DeliveryPrice = 2
            };

            // Arrange calls
            _productRepository
                .Setup(call => call.Create(It.Is<Product>(p =>
                    p.Name == product.Name &&
                    p.Description == product.Description &&
                    p.Price == product.Price &&
                    p.DeliveryPrice == product.DeliveryPrice)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _unitOfWork.Setup(prop => prop.ProductRepository).Returns(_productRepository.Object);
            _unitOfWork.Setup(call => call.Commit()).Verifiable();

            var result = await _productService.CreateProduct(product);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.Null);
        }

        [Test]
        public async Task DeleteProduct_CallsRepostory_Successful()
        {
            var productId = Guid.NewGuid();

            _productRepository
                .Setup(call => call.Delete(It.Is<Guid>(id => id == productId)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _productOptionRepository
                .Setup(call => call.DeleteByProductId(It.Is<Guid>(id => id == productId)))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _unitOfWork.Setup(prop => prop.ProductRepository).Returns(_productRepository.Object);
            _unitOfWork.Setup(prop => prop.ProductOptionRepository).Returns(_productOptionRepository.Object);
            _unitOfWork.Setup(call => call.Commit()).Verifiable();

            // Act
            await _productService.DeleteProduct(productId);
        }

        [Test]
        public async Task UpdateProduct_CallRepository_Successful()
        {
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Name = "Nike",
                Description = "Just do it",
                Price = 100,
                DeliveryPrice = 2
            };

            var originaProduct = new Product
            {
                Id = productId,
                Name = "Adidas",
                Description = "Impossible is nothing",
                Price = 80,
                DeliveryPrice = 1
            };

            _productRepository
                .Setup(call => call.GetById(It.Is<Guid>(id => id == productId)))
                .Returns(Task.FromResult(originaProduct))
                .Verifiable();

            _productRepository
                .Setup(call => call.Update(It.Is<Product>(p =>
                    p.Id == productId &&
                    p.Name == "Nike" &&
                    p.Description == "Just do it" &&
                    p.Price == (decimal)100 &&
                    p.DeliveryPrice == (decimal)2)))
                .Returns(Task.CompletedTask)
                .Verifiable();


            _unitOfWork.Setup(prop => prop.ProductRepository).Returns(_productRepository.Object);
            _unitOfWork.Setup(call => call.Commit()).Verifiable();

            // Act
            await _productService.UpdateProduct(productId, product);
        }
    }
}
