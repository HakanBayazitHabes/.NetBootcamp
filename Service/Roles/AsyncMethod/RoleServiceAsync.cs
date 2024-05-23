using System.Collections.Immutable;
using System.Net;
using System.Text.Json;
using AutoMapper;
using Repository;
using Repository.Redis;
using Repository.Roles;
using Service.Roles.DTOs;
using Service.Roles.RoleCreateUseCase;
using Service.SharedDTOs;

namespace Service.Roles.AsyncMethod;

public class RoleServiceAsync(IRoleRepositoryAsync roleRepositoryAsync, RedisService redisService, IUnitOfWork unitOfWork, IMapper mapper) : IRoleServiceAsync
{

    private const string RoleCacheKey = "roles";
    private const string RoleCacheKeyAsList = "roles-list";
    public async Task<ResponseModelDto<int>> CreateRoleAsync(RoleCreateRequestDto request)
    {
        redisService.Database.KeyDelete(RoleCacheKey);

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
        redisService.Database.KeyDelete(RoleCacheKey);

        await roleRepositoryAsync.Delete(id);

        await unitOfWork.CommitAsync();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }

    public async Task<ResponseModelDto<ImmutableList<RoleDto>>> GetAllRolesAsync()
    {
        if (redisService.Database.KeyExists(RoleCacheKey))
        {
            var roles = redisService.Database.StringGet(RoleCacheKey);

            var rolesListFromCache = JsonSerializer.Deserialize<List<RoleDto>>(roles);

            return ResponseModelDto<ImmutableList<RoleDto>>.Success(rolesListFromCache.ToImmutableList());
        }

        var roleList = await roleRepositoryAsync.GetAll();

        var roleListAsDto = mapper.Map<List<RoleDto>>(roleList);

        var roleListAsJson = JsonSerializer.Serialize(roleList);
        redisService.Database.StringSet(RoleCacheKey, roleListAsJson);

        roleListAsDto.ForEach(role =>
        {
            redisService.Database.ListLeftPush($"{RoleCacheKeyAsList}:{role.Id}", JsonSerializer.Serialize(role));
        });

        return ResponseModelDto<ImmutableList<RoleDto>>.Success(roleListAsDto.ToImmutableList());

    }

    public async Task<ResponseModelDto<RoleDto?>> GetRoleByIdAsync(int id)
    {
        var customKey = $"{RoleCacheKeyAsList}:{id}";

        if (redisService.Database.KeyExists(customKey))
        {
            var roleAsJsonFromCache = redisService.Database.ListGetByIndex(customKey, 0);

            var roleFromCache = JsonSerializer.Deserialize<RoleDto>(roleAsJsonFromCache);

            return ResponseModelDto<RoleDto?>.Success(roleFromCache);
        }

        var hasRole = await roleRepositoryAsync.GetById(id);

        // if (hasRole is null)
        // {
        //     return ResponseModelDto<RoleDto?>.Fail("Role not found", HttpStatusCode.NotFound);
        // }

        redisService.Database.ListLeftPush($"{RoleCacheKeyAsList}:{hasRole.Id}", JsonSerializer.Serialize(hasRole));

        var roleAsDto = mapper.Map<RoleDto>(hasRole);

        return ResponseModelDto<RoleDto?>.Success(roleAsDto);
    }

    public async Task<ResponseModelDto<NoContent>> UpdateRoleAsync(int roleId, RoleUpdateRequestDto request)
    {

        redisService.Database.KeyDelete(RoleCacheKey);

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
        redisService.Database.KeyDelete(RoleCacheKey);

        await roleRepositoryAsync.UpdateRoleName(name, id);

        await unitOfWork.CommitAsync();

        return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
    }
}
