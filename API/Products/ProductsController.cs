using API.Models;
using API.Products.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace NetBootcamp.API.Controllers
{
    public class ProductsController : CustomBaseController
    {
        private readonly ProductService _productService = new();

        //baseUrl/api/products
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAllWithCalculatedTax());
        }

        [HttpGet("{productId}")]
        public IActionResult GetById(int productId)
        {
            return CreateActionResult(_productService.GetByIdWithCalculatedTax(productId));
        }


        // complex type => class,record,struct => request body as Json
        // simple type => int,string,decimal => query string by default / route data

        [HttpPost]
        public IActionResult Create(ProductCreateRequestDto request)
        {
            var result = _productService.Create(request);

            return CreateActionResult(result, nameof(GetById), new { productId = result.Data });
        }

        private IActionResult CreateActionResult(object result, string v, object value)
        {
            throw new NotImplementedException();
        }

        // PUT localhost/api/products/10
        [HttpPut("{productId}")]
        public IActionResult Update(int productId, ProductUpdateRequestDto request)
        {
            return CreateActionResult(_productService.Update(productId, request));
        }

        [HttpDelete("{productId}")]
        public IActionResult Delete(int productId)
        {
            return CreateActionResult(_productService.Delete(productId));
        }
    }
}