using ToDoList.Application.Dtos;

namespace ToDoList.Application.Services
{
    public interface IToDoItemService
    {
        Task<ToDoItemStatisticsDto> GetStatisticsAsync();
    }
}
