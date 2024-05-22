namespace Repository.Roles;

public interface IRoleRepository
{
    Task<IReadOnlyList<Role>> GetAllRolesAsync();
    Task<Role> GetRoleByIdAsync(int id);
    Task CreateRoleAsync(Role role);
    Task UpdateRoleAsync(Role role);
    Task DeleteRoleAsync(int id);

}
