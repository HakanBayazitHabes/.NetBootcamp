

using Microsoft.EntityFrameworkCore;

namespace Repository.Roles;

public class RoleRepositoryAsync(AppDbContext context) : GenericRepository<Role>(context), IRoleRepositoryAsync
{
    public async Task<bool> IsExistRoleName(string roleName)
    {
        var role = await context.Roles.FirstOrDefaultAsync(role => role.Name == roleName);

        return role != null;
    }

    public async Task UpdateRoleName(string roleName, int id)
    {
        var role = await GetById(id);
        role!.Name = roleName;
        await Update(role);
    }
}
