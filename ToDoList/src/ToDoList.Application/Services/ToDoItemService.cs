using System;
using ToDoList.Application.Dtos;
using ToDoList.Domain.Entities;
using ToDoList.Application.Abstractions;
using Microsoft.EntityFrameworkCore;


namespace ToDoList.Application.Services;



public class ToDoItemService : IToDoItemService
{
    private readonly IBaseRepository<ToDoItem> _toDoRepository;


    public ToDoItemService(IBaseRepository<ToDoItem> toDoRepository)
    {
        _toDoRepository = toDoRepository;
    }

    public async Task<ToDoItem> CreateAsync(ToDoItemCreateDto dto, long userId)
    {
        var entity = new ToDoItem
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


        await _toDoRepository.AddAsync(entity);
        await _toDoRepository.SaveChangesAsync();

        return entity;
    }
}

