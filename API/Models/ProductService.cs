using System.Collections.Immutable;
using API.DTOs;

namespace API.Models;

public class ProductService
{
    private readonly ProductRepository _productRepository = new();

    public ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalculatedTax()
    {
        var productList = _productRepository.GetAll().Select(product => new ProductDto(
            product.Id,
            product.Name,
            CalculateKdv(product.Price, 1.20m),
            product.CreatedDate.ToShortDateString()
        )).ToImmutableList();

        return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
    }

    private decimal CalculateKdv(decimal price, decimal tax) => price * tax;

    public ProductDto? GetById(int id)
    {
        var product = _productRepository.GetById(id);

        if (product is null)
        {
            return null;
        }

        return new ProductDto(
            product.Id,
            product.Name,
            CalculateKdv(product.Price, 1.20m),
            product.CreatedDate.ToShortDateString()
        );
    }

    public ResponseModelDto<NoContent> Delete(int id)
    {
        var product = _productRepository.GetById(id);

        if (product is null)
        {
            return ResponseModelDto<NoContent>.Fail("Product not found");
        }

        _productRepository.Delete(id);

        return ResponseModelDto<NoContent>.Success();
    }
}