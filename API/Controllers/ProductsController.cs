using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService = new();

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(_productService.GetAllWithCalculatedTax());
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _productService.Delete(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.FailMessages);
            }

            return NoContent();
        }
    }
}