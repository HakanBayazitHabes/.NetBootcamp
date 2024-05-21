
namespace Repository;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public int Commit()
    {
        return context.SaveChanges();
    }

    public Task<int> CommitAsync()
    {
        return context.SaveChangesAsync();
    }
}
