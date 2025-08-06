using Microsoft.AspNetCore.Mvc;
using WebApp.Application.dtos.productDtos;
using WebApp.Application.services;
using WebApp.Core.entities;

namespace WebAppDemo.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly ProductService _productService;
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }


        [HttpPost("Add")]
        public async Task<ActionResult> AddAsync(AddProductDto dto)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddAsync(dto);
                return Created();
            }
            return BadRequest();

        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ReadProductDto>>> GetAllAsync()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }



        [HttpPut("Update/{id}")]
        public async Task<ActionResult> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product != null)
            {
                await _productService.UpdateAsync(dto);
                return Ok();
            }
            return NotFound();

        }


        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var product =await _productService.GetByIdAsync(id);
            if (product != null)
            {
                await _productService.RemoveAsync(id);
                return Ok();
            }
            return BadRequest();

        }



    }
}
