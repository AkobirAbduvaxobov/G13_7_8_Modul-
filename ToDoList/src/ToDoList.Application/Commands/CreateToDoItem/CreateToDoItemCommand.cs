using MediatR;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Commands.CreateToDoItem;
public record CreateToDoItemCommand(
    string Title,
    string? Description,
    PriorityLevel Priority,
    DateTime? DueDate,
    DateTime? ReminderAt
) : IRequest<long>;
