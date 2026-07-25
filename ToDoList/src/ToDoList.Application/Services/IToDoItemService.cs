namespace ToDoList.Application.Abstractions;

public interface IToDoItemService
{
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}