using Microsoft.AspNetCore.Mvc;
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

        [TearDown]
        public void TearDown()
        {
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task Get_All_CallsRepository_ReturnsProductList()
        {
            var results = new List<Product>()
            {
                new Product(){Name = "Virus"},
                new Product(){Name = "Cold"}
            };

            _productRepository.Setup(call => call.GetAll()).Returns(Task.FromResult(results.AsEnumerable())).Verifiable();

            var result = await _productsController.Get();

            Assert.That(result, Is.EqualTo(results));
        }

        [Test]
        public async Task Get_ById_RepositoryReturnsNull_ReturnsNotFound()
        {
            _productRepository.Setup(call => call.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Product)null)).Verifiable();

            var result = await _productsController.Get(id: Guid.NewGuid());

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Get_ById_ReturnsProduct()
        {
            var productId = Guid.NewGuid();
            var product = new Product() { Id = productId };

            _productRepository.Setup(call => call.GetById(It.Is<Guid>(id => id == productId))).Returns(Task.FromResult(product)).Verifiable();

            var result = await _productsController.Get(productId);

            Assert.That(result.Value, Is.EqualTo(product));
        }

        [Test]
        public async Task Post_ProductIdExists_ReturnsBadRequest()
        {
            var productId = Guid.NewGuid();

            var product = new Product() { Id = productId };

            _productRepository.Setup(call => call.GetById(It.Is<Guid>(id => id == productId))).Returns(Task.FromResult(product)).Verifiable();

            var result = await _productsController.Post(product);

            Assert.That(result.Result, Is.TypeOf<BadRequestResult>());
        }


        [Test]
        public async Task Post_ReturnsCreated()
        {
            var productId = Guid.NewGuid();

            var product = new Product() { Id = productId };

            _productRepository.Setup(call => call.GetById(It.Is<Guid>(id => id == productId))).Returns(Task.FromResult((Product)null)).Verifiable();
            _productRepository.Setup(call => call.Create(It.Is<Product>(p => p == product))).Returns(Task.CompletedTask).Verifiable();

            var result = await _productsController.Post(product);
            var createdResult = result.Result as CreatedAtActionResult;

            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.Value, Is.EqualTo(product));
            Assert.That(createdResult.ActionName, Is.EqualTo("Get"));
        }

        [Test]
        public async Task Update_WhenProductIdDoesNotMatch_ReturnsBadRequest()
        {
            var productId = Guid.NewGuid();
            var productToUpdate = new Product
            {
                Id = Guid.NewGuid()
            };

            var result = await _productsController.Update(productId, productToUpdate);

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        public async Task Update_WhenRepositoryReturnsNull_ReturnsNotFound()
        {
            var productId = Guid.NewGuid();
            var productToUpdate = new Product
            {
                Id = productId
            };

            _productRepository.Setup(call => call.GetById(It.Is<Guid>(id => id == productId))).Returns(Task.FromResult((Product)null)).Verifiable();

            var result = await _productsController.Update(productId, productToUpdate);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Update_Successful_ReturnsNoContent()
        {
            var productId = Guid.NewGuid();
            var productToUpdate = new Product
            {
                Id = productId,
                Name = "New Name",
                Description = "New Description",
                Price = 2,
                DeliveryPrice = 2
            };

            var originalProduct = new Product
            {
                Id = productId,
                Name = "Old Name",
                Description = "Old Description",
                Price = 1,
                DeliveryPrice = 1
            };

            _productRepository.Setup(call => call.GetById(It.Is<Guid>(id => id == productId))).Returns(Task.FromResult(originalProduct)).Verifiable();
            _productRepository.Setup(call => call.Update(It.Is<Product>(pr => 
                pr.Name == productToUpdate.Name &&
                pr.Description == productToUpdate.Description && 
                pr.Price == productToUpdate.Price && 
                pr.DeliveryPrice == productToUpdate.DeliveryPrice
            ))).Returns(Task.CompletedTask).Verifiable();

            var result = await _productsController.Update(productId, productToUpdate);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_RepositoryReturnsNull_ReturnsNotFound()
        {
            var productId = Guid.NewGuid();
          
            _productRepository.Setup(call => call.GetById(It.Is<Guid>(id => id == productId))).Returns(Task.FromResult((Product)null)).Verifiable();

            var result = await _productsController.Delete(productId);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Delete_Successful_ReturnsNotFound()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };

            _productRepository.Setup(call => call.GetById(It.Is<Guid>(id => id == productId))).Returns(Task.FromResult(product)).Verifiable();
            _productRepository.Setup(call => call.Delete(It.Is<Guid>(id => id == productId))).Returns(Task.CompletedTask).Verifiable();

            var result = await _productsController.Delete(productId);

            Assert.That(result.Value, Is.EqualTo(product));
        }

        [Test]
        public async Task GetOptions_CallsRepository_ReturnsOptionList()
        {
            var productId = Guid.NewGuid();
            var product = new Product() { Id = productId };
            var options = new List<ProductOption>()
            {
                new ProductOption()
                {
                    ProductId = productId,
                    Name = "Option 1"
                }
            };


            _productRepository.Setup(call => call.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(product)).Verifiable();
            _productOptionRepository.Setup(call => call.GetAll(It.Is<Guid>(id => id == productId))).Returns(Task.FromResult(options.AsEnumerable())).Verifiable();

            var result = await _productsController.GetOptions(productId);
            var okResult = result.Result as OkObjectResult;

            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(options));
        }

        [Test]
        public async Task GetOptions_RepositoryReturnsNullProduct_ReturnsNotFound()
        {
            var productId = Guid.NewGuid();
            var options = new List<ProductOption>()
            {
                new ProductOption()
                {
                    ProductId = productId,
                    Name = "Option 1"
                }
            };


            _productRepository.Setup(call => call.GetById(It.IsAny<Guid>())).Returns(Task.FromResult((Product)null)).Verifiable();
          
            var result = await _productsController.GetOptions(productId);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

    }
}