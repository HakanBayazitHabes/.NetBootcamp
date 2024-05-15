
namespace API.Roles;

public class RoleRepository : IRoleRepository
{

    private static List<Role> _roles = [
        new Role {Id = 1, Name= "Admin"},
        new Role {Id = 2, Name= "User"},
        new Role {Id = 3, Name= "Moderator"}
    ];

    public Task CreateRoleAsync(Role role)
    {
        _roles.Add(role);
        return Task.CompletedTask;
    }

    public async Task DeleteRoleAsync(int id)
    {
        var index = await GetRoleByIdAsync(id);

        if (index is null)
            return;

        _roles.Remove(index);
    }

    public Task<IReadOnlyList<Role>> GetAllRolesAsync()
    {
        return Task.FromResult<IReadOnlyList<Role>>(_roles);
    }

    public Task<Role> GetRoleByIdAsync(int id)
    {
        return Task.FromResult(_roles.Find(x => x.Id == id));
    }

    public Task UpdateRoleAsync(Role role)
    {
        var index = _roles.FindIndex(x => x.Id == role.Id);
        
        if (index <= -1)
            return Task.CompletedTask;

        _roles[index] = role;

        return Task.CompletedTask;
    }
}
