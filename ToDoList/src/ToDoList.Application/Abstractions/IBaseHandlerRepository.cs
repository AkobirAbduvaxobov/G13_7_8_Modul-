namespace ToDoList.Application.Abstractions;

public interface IBaseHandlerRepository<T> where T : class
{
    IQueryable<T> GetAllQuery();
    Task<T?> GetByIdAsync(params object[] keyValues);
}
