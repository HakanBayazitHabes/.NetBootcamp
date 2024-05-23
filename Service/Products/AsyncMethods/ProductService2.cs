using System.Collections.Immutable;
using System.Net;
using System.Text.Json;
using AutoMapper;
using Repository;
using Repository.Products;
using Repository.Products.AsyncMethods;
using Repository.Redis;
using Service.Products.DTOs;
using Service.Products.ProductCreateUseCase;
using Service.SharedDTOs;

namespace Service.Products.AsyncMethods;

public class ProductService2(IProductRepository2 productRepository, RedisService redisService, IUnitOfWork unitOfWork, IMapper mapper) : IProductService2
{
    private const string ProductCacheKey = "products";
    private const string ProductCacheKeyAsList = "products-list";

    public async Task<ResponseModelDto<int>> Create(ProductCreateRequestDto request)
    {

        redisService.Database.KeyDelete(ProductCacheKey);

        var newProduct = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            Stock = 10,
            Barcode = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.Now
        };

        await productRepository.Create(newProduct);
        await unitOfWork.CommitAsync();

        return ResponseModelDto<int>.Success(newProduct.Id, HttpStatusCode.Created);
    }

    public async Task<ResponseModelDto<NoContent>> Delete(int id)
    {
        redisService.Database.KeyDelete(ProductCacheKey);

        await productRepository.Delete(id);

        await unitOfWork.CommitAsync();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(
        PriceCalculator priceCalculator, int page, int pageSize)
    {
        var productsList = await productRepository.GetAllByPage(page, pageSize);

        var productListExample = new List<Product>()
            {
                new Product() { Id = 10, CreatedDate = DateTime.Now, Price = 100, Stock = 10, Name = "kalem 1" }
            };


        var productListAsDto = mapper.Map<List<ProductDto>>(productListExample);

        //var productListAsDto = productsList.Select(product => new ProductDto(
        //    product.Id,
        //    product.Name,
        //    priceCalculator.CalculateKdv(product.Price, 1.20m),
        //    product.Created.ToShortDateString()
        //)).ToImmutableList();


        return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
    }

    public async Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(
        PriceCalculator priceCalculator)
    {
        if (redisService.Database.KeyExists(ProductCacheKey))
        {
            var productListAsJsonFromCache = redisService.Database.StringGet(ProductCacheKey);

            var productListFromCache = JsonSerializer.Deserialize<ImmutableList<ProductDto>>(productListAsJsonFromCache);

            return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListFromCache);
        }

        var productList = await productRepository.GetAll();

        var productListAsDto = mapper.Map<List<ProductDto>>(productList);

        var productListAsJson = JsonSerializer.Serialize(productList);
        redisService.Database.StringSet(ProductCacheKey, productListAsJson);

        productListAsDto.ForEach(product =>
           {
               redisService.Database.ListLeftPush($"{ProductCacheKeyAsList}:{product.Id}",
                   JsonSerializer.Serialize(product));
           });

        return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
    }

    public async Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(int id,
        PriceCalculator priceCalculator)
    {

        var customKey = $"{ProductCacheKeyAsList}:{id}";

        if (redisService.Database.KeyExists(customKey))
        {
            var productAsJsonFromCache = redisService.Database.ListGetByIndex(customKey, 0);

            var productFromCache = JsonSerializer.Deserialize<ProductDto>(productAsJsonFromCache);

            return ResponseModelDto<ProductDto?>.Success(productFromCache);
        }

        var hasProduct = await productRepository.GetById(id);

        //Action Filter yazıldı
        // if (hasProduct is null)
        // {
        //     return ResponseModelDto<ProductDto?>.Fail("Ürün bulunamadı", HttpStatusCode.NotFound);
        // }

        // Mapper eklendi
        // var productAsDto = new ProductDto(
        //     hasProduct.Id,
        //     hasProduct.Name,
        //     priceCalculator.CalculateKdv(hasProduct.Price, 1.20m),
        //     hasProduct.CreatedDate.ToShortDateString()
        // );

        redisService.Database.ListLeftPush($"{ProductCacheKeyAsList}:{hasProduct.Id}",
                JsonSerializer.Serialize(hasProduct));


        var productAsDto = mapper.Map<ProductDto>(hasProduct);

        return ResponseModelDto<ProductDto?>.Success(productAsDto);
    }

    public async Task<ResponseModelDto<NoContent>> Update(int productId, ProductUpdateRequestDto request)
    {
        redisService.Database.KeyDelete(ProductCacheKey);

        var hasProduct = await productRepository.GetById(productId);

        // Action filter eklendi
        // if (hasProduct is null)
        // {
        //     return ResponseModelDto<NoContent>.Fail("Güncellenmeye çalışılan ürün bulunamadı.",
        //         HttpStatusCode.NotFound);
        // }


        hasProduct.Name = request.Name;
        hasProduct.Price = request.Price;


        await productRepository.Update(hasProduct);


        await unitOfWork.CommitAsync();
        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public async Task<ResponseModelDto<NoContent>> UpdateProductName(int id, string name)
    {
        redisService.Database.KeyDelete(ProductCacheKey);

        await productRepository.UpdateProductName(name, id);

        await unitOfWork.CommitAsync();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }
}
