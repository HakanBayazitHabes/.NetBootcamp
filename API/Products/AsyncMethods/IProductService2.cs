using System.Collections.Immutable;
using API.Products.DTOs;
using API.Products.ProductCreateUseCase;
using API.SharedDTOs;

namespace API.Products.AsyncMethods;

public interface IProductService2
{
    Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllWithCalculatedTax(PriceCalculator priceCalculator);


    Task<ResponseModelDto<ProductDto?>> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator);

    Task<ResponseModelDto<ImmutableList<ProductDto>>> GetAllByPageWithCalculatedTax(
        PriceCalculator priceCalculator, int page, int pageSize);


    Task<ResponseModelDto<int>> Create(ProductCreateRequestDto request);
    Task<ResponseModelDto<NoContent>> Update(int productId, ProductUpdateRequestDto request);

    Task<ResponseModelDto<NoContent>> UpdateProductName(int id, string name);


    Task<ResponseModelDto<NoContent>> Delete(int id);
}
