using ToDoList.Application.Abstractions;

namespace ToDoList.Infrastructure.Persistence.Implementations;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AppDbContext _dbContext;

    public BaseRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(T t)
    {
        await _dbContext.AddAsync(t);
    }

    public void Delete(T t)
    {
        _dbContext.Remove(t);
    }

    public IQueryable<T> GetAllQuery()
    {
        return _dbContext.Set<T>().AsQueryable();
    }

    public async Task<T?> GetByIdAsync(params object[] keyValues)
    {
        return await _dbContext.Set<T>().FindAsync(keyValues);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public void Update(T t)
    {
        _dbContext.Update(t);
    }
}
