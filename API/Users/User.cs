namespace API.Users;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Age { get; set; }

    public DateTime Created { get; set; } = new();

    public ICollection<Role> Roles { get; set; } = new List<Role>();

}
