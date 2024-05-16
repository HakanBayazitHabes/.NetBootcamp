using API.Products.DTOs;
using API.Products.ProductCreateUseCase;
using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;


namespace API.Products
{
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        private readonly IProductService _productService = productService;

        //baseUrl/api/products
        [HttpGet]
        public IActionResult GetAll([FromServices] PriceCalculator priceCalculator)
        {
            return Ok(_productService.GetAllWithCalculatedTax(priceCalculator));
        }

        [HttpGet("page/{page}/pagesize/{pageSize}")]
        public IActionResult GetAllByPage(int page, int pageSize, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(_productService.GetAllByPageWithCalculatedTax(priceCalculator, page, pageSize));
        }

        [HttpGet("{productId}")]
        public IActionResult GetById(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(_productService.GetByIdWithCalculatedTax(productId, priceCalculator));
        }


        // complex type => class,record,struct => request body as Json
        // simple type => int,string,decimal => query string by default / route data

        [HttpPost]
        public IActionResult Create(ProductCreateRequestDto request)
        {
            var result = _productService.Create(request);

            return CreateActionResult(result, nameof(GetById), new { productId = result.Data });
        }

        [HttpPut("UpdateProductName")]
        public IActionResult UpdateProductName(ProductNameUpdateRequestDto request)
        {
            return CreateActionResult(_productService.UpdateProductName(request.Id, request.Name));
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