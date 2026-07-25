using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;

namespace ToDoList.Application.Converters;

public static class ToDoItemConverter
{
    public static ToDoItem ToEntity(this ToDoItemCreateDto dto, long userId)
    {
        return new ToDoItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            ReminderAt = dto.ReminderAt,
            UserId = userId,
            IsCompleted = false,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
