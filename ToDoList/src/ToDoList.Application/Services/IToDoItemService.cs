using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services
{
    public interface IToDoItemService
    {
        Task<ToDoItem> CreateAsync(ToDoItemCreateDto dto, long userId);
    }
}