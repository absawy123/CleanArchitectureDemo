using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Application.dtos.productDtos;
using WebApp.Application.Interfaces;

namespace WebAppDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpPost("Add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetProductDto>> AddAsync(ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.AddAsync(dto);
                return Created("", result);
            }
            return BadRequest();

        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetAllAsync()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }



        [HttpPut("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateAsync(int id, ProductDto dto)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product != null)
            {
                await _productService.UpdateAsync(id,dto);
                return Ok();
            }
            return NotFound();

        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var product =await _productService.GetByIdAsync(id);
            if (product != null)
            {
                await _productService.RemoveAsync(id);
                return Ok("product has been deleted");
            }
            return BadRequest();

        }

    }
}
