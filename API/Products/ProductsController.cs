using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using Service.Products;
using Service.Products.AsyncMethods;
using Service.Products.DTOs;
using Service.Products.ProductCreateUseCase;


namespace API.Products
{
    public class ProductsController(IProductService2 productService) : CustomBaseController
    {
        private readonly IProductService2 _productService = productService;

        //baseUrl/api/products
        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] PriceCalculator priceCalculator)
        {
            var result = await _productService.GetAllWithCalculatedTax(priceCalculator);
            return Ok(result);
        }

        [HttpGet("page/{page:int}/pagesize/{pageSize:max(50)}")]
        public async Task<IActionResult> GetAllByPage(int page, int pageSize, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productService.GetAllByPageWithCalculatedTax(priceCalculator, page, pageSize));
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetById(int productId, [FromServices] PriceCalculator priceCalculator)
        {
            return CreateActionResult(await _productService.GetByIdWithCalculatedTax(productId, priceCalculator));
        }


        // complex type => class,record,struct => request body as Json
        // simple type => int,string,decimal => query string by default / route data

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequestDto request)
        {
            var result = await _productService.Create(request);

            return CreateActionResult(result, nameof(GetById), new { productId = result.Data });
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpPut("UpdateProductName")]
        public async Task<IActionResult> UpdateProductName(ProductNameUpdateRequestDto request)
        {
            return CreateActionResult(await _productService.UpdateProductName(request.Id, request.Name));
        }

        // PUT localhost/api/products/10
        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpPut("{productId:int}")]
        public async Task<IActionResult> Update(int productId, ProductUpdateRequestDto request)
        {
            return CreateActionResult(await _productService.Update(productId, request));
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> Delete(int productId)
        {
            return CreateActionResult(await _productService.Delete(productId));
        }
    }
}