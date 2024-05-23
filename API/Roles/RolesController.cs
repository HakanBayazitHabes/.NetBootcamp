using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using Service.Roles.AsyncMethod;
using Service.Roles.DTOs;
using Service.Roles.Filters;
using Service.Roles.RoleCreateUseCase;

namespace API.Roles;

public class RolesController(IRoleServiceAsync roleService) : CustomBaseController
{

    private readonly IRoleServiceAsync _roleService = roleService;

    [HttpGet]
    public async Task<IActionResult> GetAllRoles()
    {
        return CreateActionResult(await _roleService.GetAllRolesAsync());
    }

    [ServiceFilter(typeof(NotFoundFilter))]
    [HttpGet("{roleId:int}")]
    public async Task<IActionResult> GetRoleById(int roleId)
    {
        return CreateActionResult(await _roleService.GetRoleByIdAsync(roleId));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(RoleCreateRequestDto request)
    {
        var result = await _roleService.CreateRoleAsync(request);
        return CreateActionResult(result, nameof(GetRoleById), new { roleId = result.Data });
    }

    [ServiceFilter(typeof(NotFoundFilter))]
    [HttpPut("{roleId:int}")]
    public async Task<IActionResult> UpdateRole(int roleId, RoleUpdateRequestDto request)
    {
        return CreateActionResult(await _roleService.UpdateRoleAsync(roleId, request));
    }

    [ServiceFilter(typeof(NotFoundFilter))]
    [HttpDelete("{roleId:int}")]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        return CreateActionResult(await _roleService.DeleteRoleAsync(roleId));
    }

    [ServiceFilter(typeof(NotFoundFilter))]
    [HttpPut("UpdateRoleName")]
    public async Task<IActionResult> UpdateRoleName(RoleNameUpdateRequestDto request)
    {
        return CreateActionResult(await _roleService.UpdateRoleName(request.Id, request.Name));
    }
}
