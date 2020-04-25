using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xero.Products.Api.Models;
using Xero.Products.Api.Services;

namespace Xero.Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public Task<IEnumerable<Product>> Get()
        {
            return _productService.GetProducts();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public void Post(Product product)
        {
            product.Save();
        }

        [HttpPut("{id}")]
        public void Update(Guid id, Product product)
        {
            var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            var product = new Product(id);
            product.Delete();
        }

        [HttpGet("{productId}/options")]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [HttpGet("{productId}/options/{id}")]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                throw new Exception();

            return option;
        }

        [HttpPost("{productId}/options")]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [HttpPut("{productId}/options/{id}")]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete("{productId}/options/{id}")]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }
    }
}