namespace Repository.Roles;

public interface IRoleRepositoryAsync : IGenericRepository<Role>
{
    Task<bool> IsExistRoleName(string roleName);

    Task UpdateRoleName(string roleName, int id);

}
