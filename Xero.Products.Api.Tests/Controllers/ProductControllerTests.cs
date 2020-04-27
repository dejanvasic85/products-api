using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xero.Products.Api.Controllers;
using Xero.Products.Api.Mapping;
using Xero.Products.Api.Resources;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Tests
{
    public class ProductControllerTests
    {
        private ProductsController _productsController;

        private MockRepository _mockRepository;
        private Mock<IProductService> _productService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _productService = _mockRepository.Create<IProductService>();

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new ModelToResourceMapping()));
            var mapper = new Mapper(mapperConfig);

            _productsController = new ProductsController(_productService.Object, mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _mockRepository.VerifyAll();
        }

        [Test]
        public async Task Get_AllProducts_CallsService_ReturnsOk()
        {
            var products = new List<Product>()
            {
                new Product(){Name = "Virus"},
                new Product(){Name = "Cold"}
            };

            _productService
                .Setup(call => call.GetProducts(It.IsAny<string>()))
                .Returns(Task.FromResult(products.AsEnumerable()))
                .Verifiable();


            var result = await _productsController.Get();
            var items = result.Items;

            Assert.That(items.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Get_ById_RepositoryReturnsNull_ReturnsNotFound()
        {
            _productService
                .Setup(call => call.GetProductById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Product)null))
                .Verifiable();

            var result = await _productsController.Get(id: Guid.NewGuid());

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task Get_ById_ReturnsOk()
        {
            var productId = Guid.NewGuid();
            var product = new Product() { Id = productId };

            _productService
                .Setup(call => call.GetProductById(It.Is<Guid>(id => id == productId)))
                .Returns(Task.FromResult(product))
                .Verifiable();

            var result = await _productsController.Get(productId);

            Assert.That(result.Value.Id, Is.EqualTo(productId));
        }


        [Test]
        public async Task Post_ReturnsCreated()
        {
            var productId = Guid.NewGuid();

            var createProductRequest = new CreateUpdateProductResource
            {
                Name = "boom",
                Description = "shaka"
            };

            var product = new Product()
            {
                Id = productId
            };

            _productService
                .Setup(call => call.CreateProduct(It.IsAny<Product>()))
                .Returns(Task.FromResult(product))
                .Verifiable();

            var result = await _productsController.Post(createProductRequest);
            var createdResult = result.Result as CreatedAtActionResult;

            Assert.That(createdResult, Is.Not.Null);
            Assert.That(((ProductResource)createdResult.Value).Id, Is.EqualTo(productId));
            Assert.That(createdResult.ActionName, Is.EqualTo("Get"));
        }

        // ... MANY MANY more test cases to come here
    }
}