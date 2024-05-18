using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T> where T : class
{

    public DbSet<T> DbSet { get; set; } = context.Set<T>();

    public async Task<IReadOnlyList<T>> GetAll()
    {
        var list = await DbSet.ToListAsync();
        return list.AsReadOnly();
    }

    public async Task<IReadOnlyList<T>> GetAll(Expression<Func<T, bool>> predicate)
    {
        var list = await DbSet.Where(predicate).ToListAsync();
        return list.AsReadOnly();
    }

    public async Task<IReadOnlyList<T>> GetAllByPage(int page, int pageSize)
    {
        var list = await DbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return list.AsReadOnly();
    }

    public async Task<T> Create(T entity)
    {
        await DbSet.AddAsync(entity);

        return entity;
    }

    public async Task Delete(int id)
    {
        var entity = await GetById(id);

        DbSet.Remove(entity);
    }


    public async Task<T> GetById(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public Task Update(T entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }
}
