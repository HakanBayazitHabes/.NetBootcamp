using AutoMapper;
using Repository.Products;
using Service.Products.DTOs;

namespace Service.Products;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductDto>()
        // .ReverseMap();
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(y => new PriceCalculator().CalculateKdv(y.Price, 1.20m)));
    }
}
