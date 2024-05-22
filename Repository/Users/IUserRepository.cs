namespace Repository.Users;

public interface IUserRepository
{
    IReadOnlyList<User> GetAll();
    void Create(User user);
    void Update(User user);
    void Delete(int id);
    User? GetById(int id);
    
}
