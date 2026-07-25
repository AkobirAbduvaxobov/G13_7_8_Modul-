using ToDoList.Application.Dtos;

namespace ToDoList.Application.Services
{
    public interface IToDoItemService
    {
        Task<List<ToDoItemDto>> Search(string searchTerm);
    }
}