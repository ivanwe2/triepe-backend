using Autofac.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Triepe.Domain.Abstractions.Services;
using Triepe.Domain.Dtos.ProductDtos;
using Triepe.Domain.Pagination;

namespace Triepe.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> CreateProductAsync([FromBody] ProductRequestDto product)
        {
            var createdProduct = await _productService.CreateAsync(product);

            return CreatedAtAction(nameof(GetProductAsync), new{ id = createdProduct.Id }, createdProduct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductAsync([FromRoute] Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetPageAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var activities = await _productService.GetPaginatedResultAsync(pagingInfo);

            return Ok(activities);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditProductAsync([FromRoute] Guid id, [FromBody] ProductRequestDto Product)
        {
            await _productService.UpdateAsync(id, Product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync([FromRoute] Guid id)
        {
            await _productService.DeleteAsync(id);

            return NoContent();
        }
    }
}
