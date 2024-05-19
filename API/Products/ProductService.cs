using System.Collections.Immutable;
using System.Net;
using API.Products.DTOs;
using API.Products.ProductCreateUseCase;
using API.Repositories;
using API.SharedDTOs;

namespace API.Products;

public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
{
    public ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalculatedTax(PriceCalculator priceCalculator)
    {
        var productList = productRepository.GetAll().Select(product => new ProductDto(
            product.Id,
            product.Name,
            priceCalculator.CalculateKdv(product.Price, 1.20m),
            product.CreatedDate.ToShortDateString()
        )).ToImmutableList();

        return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
    }

    public ResponseModelDto<ImmutableList<ProductDto>> GetAllByPageWithCalculatedTax(
                PriceCalculator priceCalculator, int page, int pageSize)
    {
        var productList = productRepository.GetAllByPage(page, pageSize).Select(product => new ProductDto(
            product.Id,
            product.Name,
            priceCalculator.CalculateKdv(product.Price, 1.20m),
            product.CreatedDate.ToShortDateString()
        )).ToImmutableList();


        return ResponseModelDto<ImmutableList<ProductDto>>.Success(productList);
    }



    public ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator)
    {
        var product = productRepository.GetById(id);

        if (product is null)
        {
            return ResponseModelDto<ProductDto?>.Fail("Product Not Found", HttpStatusCode.NotFound);
        }

        var newDto = new ProductDto(
            product.Id,
            product.Name,
            priceCalculator.CalculateKdv(product.Price, 1.20m),
            product.CreatedDate.ToShortDateString()
        );

        return ResponseModelDto<ProductDto?>.Success(newDto);
    }

    public ResponseModelDto<NoContent> Delete(int id)
    {
        var product = productRepository.GetById(id);

        if (product is null)
        {
            return ResponseModelDto<NoContent>.Fail("Product not found",
                    HttpStatusCode.NotFound);
        }

        productRepository.Delete(id);

        unitOfWork.Commit();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public ResponseModelDto<int> Create(ProductCreateRequestDto request)
    {
        // fast fail
        // Guard clauses

        //var hasProduct = productRepository.IsExists(request.Name.Trim());

        //if (hasProduct)
        //{
        //    return ResponseModelDto<int>.Fail("Oluşturma çalıştığınız ürün bulunmaktadır.",
        //        HttpStatusCode.BadRequest);
        //}

        var newProduct = new Product
        {
            //Id = productRepository.GetAll().Count + 1,
            Name = request.Name.Trim(),
            Price = request.Price,
            Stock = 10,
            Barcode = Guid.NewGuid().ToString(),
            CreatedDate = DateTime.Now
        };

        productRepository.Create(newProduct);

        unitOfWork.Commit();

        return ResponseModelDto<int>.Success(newProduct.Id, HttpStatusCode.Created);
    }

    public ResponseModelDto<NoContent> UpdateProductName(int productId, string name)
    {
        var hasProduct = productRepository.GetById(productId);

        if (hasProduct is null)
        {
            return ResponseModelDto<NoContent>.Fail("Güncellenmeye çalışılan ürün bulunamadı.",
                HttpStatusCode.NotFound);
        }

        //productRepository.UpdateProductName(name, productId);
        unitOfWork.Commit();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public ResponseModelDto<NoContent> Update(int productId, ProductUpdateRequestDto request)
    {
        var hasProduct = productRepository.GetById(productId);

        if (hasProduct is null)
        {
            return ResponseModelDto<NoContent>.Fail("Güncellenmeye çalışılan ürün bulunamadı.",
                HttpStatusCode.NotFound);
        }

        hasProduct.Name = request.Name;
        hasProduct.Price = request.Price;

        productRepository.Update(hasProduct);

        unitOfWork.Commit();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

}