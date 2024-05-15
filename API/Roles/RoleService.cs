using System.Collections.Immutable;
using System.Net;
using API.Products;
using API.Roles.DTOs;
using API.SharedDTOs;

namespace API.Roles;

public class RoleService(IRoleRepository roleRepository) : IRoleService
{
    public Task<ResponseModelDto<int>> CreateRoleAsync(RoleCreateRequestDto request)
    {
        var newRole = new Role
        {
            Id = roleRepository.GetAllRolesAsync().Result.Count + 1,
            Name = request.Name
        };

        roleRepository.CreateRoleAsync(newRole);

        return Task.FromResult(ResponseModelDto<int>.Success(newRole.Id, HttpStatusCode.Created));
    }

    public async Task<ResponseModelDto<NoContent>> DeleteRoleAsync(int id)
    {
        var role = await roleRepository.GetRoleByIdAsync(id);

        if (role is null)
            return ResponseModelDto<NoContent>.Fail("Role not Found", HttpStatusCode.NotFound);

        await roleRepository.DeleteRoleAsync(id);

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public async Task<ResponseModelDto<ImmutableList<RoleDto>>> GetAllRolesAsync()
    {
        var roleList = (await roleRepository.GetAllRolesAsync()).Select(role => new RoleDto(role.Id, role.Name)).ToImmutableList();

        return ResponseModelDto<ImmutableList<RoleDto>>.Success(roleList);
    }

    public async Task<ResponseModelDto<RoleDto?>> GetRoleByIdAsync(int id)
    {
        var role = await roleRepository.GetRoleByIdAsync(id);

        if (role is null)
            return ResponseModelDto<RoleDto?>.Fail("Role not Found", HttpStatusCode.NotFound);

        var newDto = new RoleDto(role.Id, role.Name);


        return ResponseModelDto<RoleDto?>.Success(newDto);

    }

    public Task<ResponseModelDto<NoContent>> UpdateRoleAsync(int roleId, RoleUpdateRequestDto request)
    {
        var hasRole = roleRepository.GetRoleByIdAsync(roleId);
        if (hasRole is null)
            return Task.FromResult(ResponseModelDto<NoContent>.Fail("Role not Found", HttpStatusCode.NotFound));

        var updateRole = new Role
        {
            Id = roleId,
            Name = request.Name
        };

        roleRepository.UpdateRoleAsync(updateRole);

        return Task.FromResult(ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent));

    }
}
