using ToDoList.Application.Abstractions;
using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IBaseRepository<ToDoItem> _toDoItemRepository;

        public ToDoItemService(IBaseRepository<ToDoItem> toDoItemRepository)
        {
            _toDoItemRepository = toDoItemRepository;
        }

        public Task<ToDoItemStatisticsDto> GetStatisticsAsync()
        {
            var toDoItems = _toDoItemRepository.GetAllQuery()
                .Where(t => !t.IsDeleted)
                .ToList();

            var now = DateTime.UtcNow;

            var statistics = new ToDoItemStatisticsDto
            {
                TotalCount = toDoItems.Count,
                CompletedCount = toDoItems.Count(t => t.IsCompleted),
                PendingCount = toDoItems.Count(t => !t.IsCompleted),
                OverdueCount = toDoItems.Count(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < now),
                CountByPriority = toDoItems
                    .GroupBy(t => t.Priority)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count()),
            };

            return Task.FromResult(statistics);
        }
    }
}
