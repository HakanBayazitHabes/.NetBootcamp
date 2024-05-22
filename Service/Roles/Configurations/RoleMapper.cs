using AutoMapper;
using Repository.Roles;
using Service.Roles.DTOs;

namespace Service.Roles.Configurations;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}
