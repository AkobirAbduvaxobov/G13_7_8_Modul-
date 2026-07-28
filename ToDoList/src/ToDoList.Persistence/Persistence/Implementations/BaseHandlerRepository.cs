using ToDoList.Application.Abstractions;

namespace ToDoList.Infrastructure.Persistence.Implementations;

public class BaseHandlerRepository<T> : IBaseHandlerRepository<T> where T : class
{
    private readonly AppDbContext _dbContext;

    public BaseHandlerRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> GetAllQuery()
    {
        return _dbContext.Set<T>().AsQueryable();
    }

    public async Task<T?> GetByIdAsync(params object[] keyValues)
    {
        return await _dbContext.Set<T>().FindAsync(keyValues);
    }
}
