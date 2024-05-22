using AutoMapper;
using Repository.Users;
using Service.Users.DTOs;
using Service.Users.Helpers;

namespace Service.Users.Configurations;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => new AgeCalculator().CalculateAge(src.Age)))
            .ReverseMap();
    }
}
