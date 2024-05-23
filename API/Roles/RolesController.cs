using Microsoft.AspNetCore.Mvc;
using NetBootcamp.API.Controllers;
using Service.Roles.AsyncMethod;
using Service.Roles.DTOs;
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

    [HttpGet("{roleId}")]
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

    [HttpPut("{roleId}")]
    public async Task<IActionResult> UpdateRole(int roleId, RoleUpdateRequestDto request)
    {
        return CreateActionResult(await _roleService.UpdateRoleAsync(roleId, request));
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        return CreateActionResult(await _roleService.DeleteRoleAsync(roleId));
    }
}
