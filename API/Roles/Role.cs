using API.Users;

namespace API.Roles;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
}
