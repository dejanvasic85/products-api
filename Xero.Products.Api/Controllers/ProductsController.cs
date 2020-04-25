using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xero.Products.Api.Models;
using Xero.Products.Api.Repository;

namespace Xero.Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository _productRepository;
        private IProductOptionRepository _productOptionRepository;

        public ProductsController(IProductRepository productRepository, IProductOptionRepository productOptionRepository)
        {
            _productRepository = productRepository;
            _productOptionRepository = productOptionRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _productRepository.GetAllProducts();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            var existingProduct = await _productRepository.GetProductById(product.Id);
            if (existingProduct != null)
            {
                return BadRequest();
            }

            await _productRepository.CreateProduct(product);

            return CreatedAtAction(nameof(Get), new { product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var original = await _productRepository.GetProductById(id);

            if (original == null)
            {
                return NotFound();
            }

            original.Name = product.Name;
            original.Description = product.Description;
            original.Price = product.Price;
            original.DeliveryPrice = product.DeliveryPrice;

            await _productRepository.UpdateProduct(original);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(Guid id)
        {
            var product = await _productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteProduct(id);

            return product;
        }

        [HttpGet("{productId}/options")]
        public async Task<ActionResult<IEnumerable<ProductOption>>> GetOptions(Guid productId)
        {
            var product = await _productRepository.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            var options = await _productOptionRepository.GetProductOptions(productId);

            return Ok(options);
        }

        [HttpGet("{productId}/options/{id}")]
        public async Task<ActionResult<ProductOption>> GetOption(Guid productId, Guid id)
        {
            var option = await _productOptionRepository.GetProductOption(productId, id);

            if (option == null)
            {
                return NotFound();
            }

            return option;
        }

        [HttpPost("{productId}/options")]
        public async Task<ActionResult<ProductOption>> CreateOption(Guid productId, ProductOption option)
        {
            var existingOption = await _productOptionRepository.GetProductOption(productId, option.Id);
            if (existingOption != null)
            {
                return BadRequest();
            }

            await _productOptionRepository.CreateProductOption(option);

            return CreatedAtAction(nameof(GetOption), new { productId, id = option.Id }, option);
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