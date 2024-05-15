namespace API.Users;

public class UserRepository : IUserRepository
{
    private static List<User> _users = [
        new User { Id = 1, Name = "User 1", Email = "user1@gmail.com", Password = "user1Password12*"},
        new User { Id = 2, Name = "User 2", Email = "user2@gmail.com", Password = "user2Password12*"},
        new User { Id = 3, Name = "User 3", Email = "user3@gmail.com", Password = "user3Password12*"},
        new User { Id = 4, Name = "User 4", Email = "user4@gmail.com", Password = "user4Password12*"},
        new User { Id = 5, Name = "User 5", Email = "user5@gmail.com", Password = "user5Password12*"},
        new User { Id = 6, Name = "User 6", Email = "user6@gmail.com", Password = "user6Password12*"},
    ];

    public void Create(User user)
    {
        var methodName = nameof(UsersController.GetById);
        _users.Add(user);
    }

    public void Delete(int id)
    {
        var user = GetById(id);

        if (user is null)
            return;

        _users.Remove(user);
    }

    public IReadOnlyList<User> GetAll()
    {
        return _users;
    }

    public User? GetById(int id)
    {
        return _users.Find(x => x.Id == id);
    }

    public void Update(User user)
    {
        var index = _users.FindIndex(x => x.Id == user.Id);

        _users[index] = user;
    }
}
