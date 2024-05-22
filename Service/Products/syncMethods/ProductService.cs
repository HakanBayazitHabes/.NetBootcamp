using System.Collections.Immutable;
using System.Net;
using AutoMapper;
using Repository;
using Repository.Products;
using Service.Products.DTOs;
using Service.Products.ProductCreateUseCase;
using Service.SharedDTOs;

namespace Service.Products;

public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    public ResponseModelDto<ImmutableList<ProductDto>> GetAllWithCalculatedTax(PriceCalculator priceCalculator)
    {
        var productList = productRepository.GetAll();
        // .Select(product => new ProductDto(
        //     product.Id,
        //     product.Name,
        //     priceCalculator.CalculateKdv(product.Price, 1.20m),
        //     product.CreatedDate.ToShortDateString()
        // )).ToImmutableList();

        var productListAsDto = mapper.Map<List<ProductDto>>(productList);

        return ResponseModelDto<ImmutableList<ProductDto>>.Success(productListAsDto.ToImmutableList());
    }

    public ResponseModelDto<ImmutableList<ProductDto>> GetAllByPageWithCalculatedTax(
                PriceCalculator priceCalculator, int page, int pageSize)
    {
        var productList = productRepository.GetAllByPage(page, pageSize);
        // .Select(product => new ProductDto(
        //     product.Id,
        //     product.Name,
        //     priceCalculator.CalculateKdv(product.Price, 1.20m),
        //     product.CreatedDate.ToShortDateString()
        // )).ToImmutableList();

        var prodcutListAsDto = mapper.Map<List<ProductDto>>(productList);


        return ResponseModelDto<ImmutableList<ProductDto>>.Success(prodcutListAsDto.ToImmutableList());
    }



    public ResponseModelDto<ProductDto?> GetByIdWithCalculatedTax(int id, PriceCalculator priceCalculator)
    {
        var product = productRepository.GetById(id);

        if (product is null)
        {
            return ResponseModelDto<ProductDto?>.Fail("Product Not Found", HttpStatusCode.NotFound);
        }

        // var newDto = new ProductDto(
        //     product.Id,
        //     product.Name,
        //     priceCalculator.CalculateKdv(product.Price, 1.20m),
        //     product.CreatedDate.ToShortDateString()
        // );

        var productAsDto = mapper.Map<ProductDto>(product);

        return ResponseModelDto<ProductDto?>.Success(productAsDto);
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