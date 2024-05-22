using System.Collections.Immutable;
using Service.Roles.DTOs;
using Service.Roles.RoleCreateUseCase;
using Service.SharedDTOs;

namespace Service.Roles.AsyncMethod;

public interface IRoleServiceAsync
{
    Task<ResponseModelDto<ImmutableList<RoleDto>>> GetAllRolesAsync();
    Task<ResponseModelDto<RoleDto?>> GetRoleByIdAsync(int id);

    Task<ResponseModelDto<int>> CreateRoleAsync(RoleCreateRequestDto request);

    Task<ResponseModelDto<NoContent>> UpdateRoleAsync(int roleId, RoleUpdateRequestDto request);

    Task<ResponseModelDto<NoContent>> DeleteRoleAsync(int id);

    Task<ResponseModelDto<NoContent>> UpdateRoleName(int id, string name);

}
