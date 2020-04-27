using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

using Xero.Products.Api.Resources;
using Xero.Products.BusinessLayer;

namespace Xero.Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ListResponse<ProductResource>> Get(string name = "")
        {
            var products = await _productService.GetProducts(name);
            var productResources = products.Select(p => _mapper.Map<Product, ProductResource>(p));
            return new ListResponse<ProductResource>(productResources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResource>> Get(Guid id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return _mapper.Map<Product, ProductResource>(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResource>> Post(CreateUpdateProductResource productCreateResource)
        {
            var product = _mapper.Map<CreateUpdateProductResource, Product>(productCreateResource);
            var newProduct = await _productService.CreateProduct(product);
            var productResource = _mapper.Map<Product, ProductResource>(newProduct);

            return CreatedAtAction(nameof(Get), new { newProduct.Id }, productResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, CreateUpdateProductResource productResource)
        {
            var original = await _productService.GetProductById(id);

            if (original == null)
            {
                return NotFound();
            }

            var productToUpdate = _mapper.Map<CreateUpdateProductResource, Product>(productResource);

            await _productService.UpdateProduct(id, productToUpdate);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductResource>> Delete(Guid id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productService.DeleteProduct(id);

            return _mapper.Map<Product, ProductResource>(product);
        }

        [HttpGet("{productId}/options")]
        public async Task<ActionResult<ListResponse<ProductOptionResource>>> GetOptions(Guid productId)
        {
            var product = await _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            var options = await _productService.GetProductOptions(productId);
            var optionResources = options.Select(opt => _mapper.Map<ProductOption, ProductOptionResource>(opt));
            var listResponse = new ListResponse<ProductOptionResource>(optionResources);

            return Ok(listResponse);
        }

        [HttpGet("{productId}/options/{id}")]
        public async Task<ActionResult<ProductOptionResource>> GetOption(Guid productId, Guid id)
        {
            var option = await _productService.GetProductOption(productId, id);

            if (option == null)
            {
                return NotFound();
            }

            return _mapper.Map<ProductOption, ProductOptionResource>(option);
        }

        [HttpPost("{productId}/options")]
        public async Task<ActionResult<ProductOptionResource>> CreateOption(Guid productId, CreateUpdateProductOptionResource productOptionResource)
        {
            var product = await _productService.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }

            var newOption = _mapper.Map<CreateUpdateProductOptionResource, ProductOption>(productOptionResource);
            var createdOption = await _productService.CreateProductOption(productId, newOption);

            return CreatedAtAction(nameof(GetOption), new { productId, id = newOption.Id }, _mapper.Map<ProductOption, ProductOptionResource>(createdOption));
        }

        [HttpPut("{productId}/options/{id}")]
        public async Task<ActionResult> UpdateOption(Guid productId, Guid id, CreateUpdateProductOptionResource productOptionResource)
        { 
            var originalOption = await _productService.GetProductOption(productId, id);
            if (originalOption == null)
            {
                return NotFound();
            }

            var optionToUpdate = _mapper.Map<CreateUpdateProductOptionResource, ProductOption>(productOptionResource);
            await _productService.UpdateProductOption(productId, id, optionToUpdate);

            return NoContent();
        }

        [HttpDelete("{productId}/options/{id}")]
        public async Task<ActionResult<ProductOptionResource>> DeleteOption(Guid productId, Guid id)
        {
            var productOption = await _productService.GetProductOption(productId, id);

            if (productOption == null)
            {
                return NotFound();
            }

            await _productService.DeleteProductOption(id);

            return _mapper.Map<ProductOption, ProductOptionResource>(productOption);
        }
    }
}