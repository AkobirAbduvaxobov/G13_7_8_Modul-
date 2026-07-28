namespace ToDoList.Application.Abstractions;

public interface IBaseCommandRepository<T> where T : class
{
    Task AddAsync(T t);
    void Update(T t);
    void Delete(T t);
    Task<int> SaveChangesAsync();
}
