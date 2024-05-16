using System.Collections.Immutable;
using API.Products.DTOs;
using API.Products.ProductCreateUseCase;
using API.SharedDTOs;

namespace API.Products;

public interface IProductService
{

    ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalculatedTax(PriceCalculator priceCalculator);
    ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator);
    ResponseModelDto<ImmutableList<ProductDto>> GetAllByPageWithCalculatedTax(
            PriceCalculator priceCalculator, int page, int pageSize);
    ResponseModelDto<int> Create(ProductCreateRequestDto request);
    ResponseModelDto<NoContent> Update(int productId, ProductUpdateRequestDto request);
    ResponseModelDto<NoContent> UpdateProductName(int id, string name);

    ResponseModelDto<NoContent> Delete(int id);

}
