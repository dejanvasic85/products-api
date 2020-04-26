﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xero.Products.BusinessLayer;

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
            return await _productRepository.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(Guid id)
        {
            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            var existingProduct = await _productRepository.GetById(product.Id);
            if (existingProduct != null)
            {
                return BadRequest();
            }

            await _productRepository.Create(product);

            return CreatedAtAction(nameof(Get), new { product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            var original = await _productRepository.GetById(id);

            if (original == null)
            {
                return NotFound();
            }

            original.Name = product.Name;
            original.Description = product.Description;
            original.Price = product.Price;
            original.DeliveryPrice = product.DeliveryPrice;

            await _productRepository.Update(original);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> Delete(Guid id)
        {
            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.Delete(id);

            return product;
        }

        [HttpGet("{productId}/options")]
        public async Task<ActionResult<IEnumerable<ProductOption>>> GetOptions(Guid productId)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null)
            {
                return NotFound();
            }

            var options = await _productOptionRepository.GetAll(productId);

            return Ok(options);
        }

        [HttpGet("{productId}/options/{id}")]
        public async Task<ActionResult<ProductOption>> GetOption(Guid productId, Guid id)
        {
            var option = await _productOptionRepository.GetById(productId, id);

            if (option == null)
            {
                return NotFound();
            }

            return option;
        }

        [HttpPost("{productId}/options")]
        public async Task<ActionResult<ProductOption>> CreateOption(Guid productId, ProductOption option)
        {
            var existingOption = await _productOptionRepository.GetById(productId, option.Id);
            if (existingOption != null)
            {
                return BadRequest();
            }

            await _productOptionRepository.Create(option);

            return CreatedAtAction(nameof(GetOption), new { productId, id = option.Id }, option);
        }

        [HttpPut("{productId}/options/{id}")]
        public async Task<ActionResult> UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            if (id != option.Id)
            {
                return BadRequest();
            }

            var originalOption = await _productOptionRepository.GetById(productId, id);
            if (originalOption == null)
            {
                return NotFound();
            }

            originalOption.Name = option.Name;
            originalOption.Description = option.Description;

            await _productOptionRepository.Update(option);

            return NoContent();
        }

        [HttpDelete("{productId}/options/{id}")]
        public async Task<ActionResult<ProductOption>> DeleteOption(Guid productId, Guid id)
        {
            var productOption = await _productOptionRepository.GetById(productId, id);

            if (productOption == null)
            {
                return NotFound();
            }

            await _productOptionRepository.Delete(id);

            return productOption;
        }
    }
}