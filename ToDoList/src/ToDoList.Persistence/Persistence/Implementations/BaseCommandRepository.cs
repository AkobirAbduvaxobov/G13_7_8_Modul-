using ToDoList.Application.Abstractions;

namespace ToDoList.Infrastructure.Persistence.Implementations;

public class BaseCommandRepository<T> : IBaseCommandRepository<T> where T : class
{
    private readonly AppDbContext _dbContext;

    public BaseCommandRepository(AppDbContext dbContext)
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

    public void Update(T t)
    {
        _dbContext.Update(t);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
