using System.Collections.Immutable;
using System.Net;
using AutoMapper;
using Repository;
using Repository.Roles;
using Service.Roles.DTOs;
using Service.Roles.RoleCreateUseCase;
using Service.SharedDTOs;

namespace Service.Roles.AsyncMethod;

public class RoleServiceAsync(IRoleRepositoryAsync roleRepositoryAsync, IUnitOfWork unitOfWork, IMapper mapper) : IRoleServiceAsync
{
    public async Task<ResponseModelDto<int>> CreateRoleAsync(RoleCreateRequestDto request)
    {
        var newRole = new Role
        {
            Name = request.Name,
        };

        await roleRepositoryAsync.Create(newRole);
        await unitOfWork.CommitAsync();

        return ResponseModelDto<int>.Success(newRole.Id, HttpStatusCode.Created);

    }

    public async Task<ResponseModelDto<NoContent>> DeleteRoleAsync(int id)
    {
        await roleRepositoryAsync.Delete(id);
        await unitOfWork.CommitAsync();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public async Task<ResponseModelDto<ImmutableList<RoleDto>>> GetAllRolesAsync()
    {
        var roleList = await roleRepositoryAsync.GetAll();

        var roleListAsDto = mapper.Map<List<RoleDto>>(roleList);

        return ResponseModelDto<ImmutableList<RoleDto>>.Success(roleListAsDto.ToImmutableList());

    }

    public async Task<ResponseModelDto<RoleDto?>> GetRoleByIdAsync(int id)
    {
        var hasRole = await roleRepositoryAsync.GetById(id);

        // if (hasRole is null)
        // {
        //     return ResponseModelDto<RoleDto?>.Fail("Role not found", HttpStatusCode.NotFound);
        // }

        var roleAsDto = mapper.Map<RoleDto>(hasRole);

        return ResponseModelDto<RoleDto?>.Success(roleAsDto);
    }

    public async Task<ResponseModelDto<NoContent>> UpdateRoleAsync(int roleId, RoleUpdateRequestDto request)
    {
        var hasRole = await roleRepositoryAsync.GetById(roleId);

        // if (hasRole is null)
        // {
        //     return ResponseModelDto<NoContent>.Fail("Role not found", HttpStatusCode.NotFound);
        // }

        hasRole.Name = request.Name;

        await roleRepositoryAsync.Update(hasRole);
        await unitOfWork.CommitAsync();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public async Task<ResponseModelDto<NoContent>> UpdateRoleName(int id, string name)
    {
        await roleRepositoryAsync.UpdateRoleName(name, id);

        await unitOfWork.CommitAsync();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }
}
