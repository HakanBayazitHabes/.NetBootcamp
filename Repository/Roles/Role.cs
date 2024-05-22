using Repository.Users;

namespace Repository.Roles;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}
