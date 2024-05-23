using Repository.Users;

namespace Repository.Roles;

public class Role : BaseEntity<int>
{
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}
