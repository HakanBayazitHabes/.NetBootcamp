using System.Collections.Immutable;
using System.Net;
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


    public ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id)
    {
        var product = _productRepository.GetById(id);

        if (product is null)
        {
            return ResponseModelDto<ProductDto?>.Fail("Product Not Found", HttpStatusCode.NotFound);
        }

        var newDto = new ProductDto(
            product.Id,
            product.Name,
            CalculateKdv(product.Price, 1.20m),
            product.CreatedDate.ToShortDateString()
        );

        return ResponseModelDto<ProductDto?>.Success(newDto);
    }

    public ResponseModelDto<NoContent> Delete(int id)
    {
        var product = _productRepository.GetById(id);

        if (product is null)
        {
            return ResponseModelDto<NoContent>.Fail("Product not found",
                    HttpStatusCode.NotFound);
        }

        _productRepository.Delete(id);

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public ResponseModelDto<int> Create(ProductCreateRequestDto request)
    {
        var newProduct = new Product
        {
            Id = _productRepository.GetAll().Count + 1,
            Name = request.Name,
            Price = request.price,
            CreatedDate = DateTime.Now
        };

        _productRepository.Create(newProduct);

        return ResponseModelDto<int>.Success(newProduct.Id, HttpStatusCode.Created);
    }

    public ResponseModelDto<NoContent> Update(int productId, ProductUpdateRequestDto request)
    {
        var hasProduct = _productRepository.GetById(productId);

        if (hasProduct is null)
        {
            return ResponseModelDto<NoContent>.Fail("Güncellenmeye çalışılan ürün bulunamadı.",
                HttpStatusCode.NotFound);
        }

        var updatedProduct = new Product
        {
            Id = productId,
            Name = request.Name,
            Price = request.Price,
            CreatedDate = hasProduct.CreatedDate
        };

        _productRepository.Update(updatedProduct);

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }


    private decimal CalculateKdv(decimal price, decimal tax) => price * tax;

}