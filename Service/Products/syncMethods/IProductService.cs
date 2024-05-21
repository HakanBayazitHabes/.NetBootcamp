using System.Collections.Immutable;
using Service.Products.DTOs;
using Service.Products.ProductCreateUseCase;
using Service.SharedDTOs;

namespace Service.Products;

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
