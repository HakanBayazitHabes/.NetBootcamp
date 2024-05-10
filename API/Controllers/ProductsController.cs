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
    }
}