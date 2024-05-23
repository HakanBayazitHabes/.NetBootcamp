using Repository.Roles;

namespace Repository.Users;

public class User : BaseEntity<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Age { get; set; }

    public DateTime CreatedDate { get; set; } = new();

    public ICollection<Role> Roles { get; set; }

}
