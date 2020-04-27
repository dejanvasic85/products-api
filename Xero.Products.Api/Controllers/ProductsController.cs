using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Xero.Products.Api.Resources;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ListResponse<Product>> Get(string name = "")
        {
            var products = await _productService.GetProducts(name);
            return new ListResponse<Product>(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            var newProduct = await _productService.CreateProduct(product);

            return CreatedAtAction(nameof(Get), new { newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var original = await _productService.GetProductById(id);

            if (original == null)
            {
                return NotFound();
            }

            original.Name = product.Name;
            original.Description = product.Description;
            original.Price = product.Price;
            original.DeliveryPrice = product.DeliveryPrice;

            await _productService.UpdateProduct(original);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(Guid id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProduct(id);

            return product;
        }

        [HttpGet("{productId}/options")]
        public async Task<ActionResult<ListResponse<ProductOption>>> GetOptions(Guid productId)
        {
            var product = await _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            var options = await _productService.GetProductOptions(productId);
            var listResponse = new ListResponse<ProductOption>(options);

            return Ok(listResponse);
        }

        [HttpGet("{productId}/options/{id}")]
        public async Task<ActionResult<ProductOption>> GetOption(Guid productId, Guid id)
        {
            var option = await _productService.GetProductOption(productId, id);

            if (option == null)
            {
                return NotFound();
            }

            return option;
        }

        [HttpPost("{productId}/options")]
        public async Task<ActionResult<ProductOption>> CreateOption(Guid productId, ProductOption option)
        {
            var existingOption = await _productService.GetProductOption(productId, option.Id);
            if (existingOption != null)
            {
                return BadRequest();
            }

            await _productService.CreateProductOption(option);

            return CreatedAtAction(nameof(GetOption), new { productId, id = option.Id }, option);
        }

        [HttpPut("{productId}/options/{id}")]
        public async Task<ActionResult> UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            if (id != option.Id)
            {
                return BadRequest();
            }

            var originalOption = await _productService.GetProductOption(productId, id);
            if (originalOption == null)
            {
                return NotFound();
            }

            await _productService.UpdateProductOption(productId, option);

            return NoContent();
        }

        [HttpDelete("{productId}/options/{id}")]
        public async Task<ActionResult<ProductOption>> DeleteOption(Guid productId, Guid id)
        {
            var productOption = await _productService.GetProductOption(productId, id);

            if (productOption == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductOption(id);

            return productOption;
        }
    }
}