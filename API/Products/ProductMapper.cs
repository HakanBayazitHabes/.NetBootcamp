using API.Products.DTOs;
using AutoMapper;

namespace API.Products;

public class ProductMapper : Profile
{
    public ProductMapper()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(y => new PriceCalculator().CalculateKdv(y.Price, 1.20m)));
    }
}
