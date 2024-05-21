using System.Collections.Immutable;
using API.Roles.DTOs;
using Service.SharedDTOs;

namespace API.Roles;

public interface IRoleService
{
    Task<ResponseModelDto<ImmutableList<RoleDto>>> GetAllRolesAsync();
    Task<ResponseModelDto<RoleDto?>> GetRoleByIdAsync(int id);

    Task<ResponseModelDto<int>> CreateRoleAsync(RoleCreateRequestDto request);

    Task<ResponseModelDto<NoContent>> UpdateRoleAsync(int roleId, RoleUpdateRequestDto request);

    Task<ResponseModelDto<NoContent>> DeleteRoleAsync(int id);

}
