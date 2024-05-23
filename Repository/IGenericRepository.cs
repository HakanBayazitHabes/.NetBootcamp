using System.Linq.Expressions;

namespace Repository;

public interface IGenericRepository<T>
{
    Task<IReadOnlyList<T>> GetAll();
    Task<IReadOnlyList<T>> GetAll(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetAllByPage(int page, int pageSize);
    Task Update(T entity);

    Task<T> Create(T entity);
    Task<T> GetById(int id);
    Task Delete(int id);
    Task<bool> HasExist(int id);

}
