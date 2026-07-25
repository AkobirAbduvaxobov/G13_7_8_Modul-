using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Application.Services;

public class ToDoItemService : IToDoItemService
{
    private readonly IBaseRepository<ToDoList.Domain.Entities.ToDoItem> _repository;

    public ToDoItemService(IBaseRepository<ToDoList.Domain.Entities.ToDoItem> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<object>> GetAllAsync()
    {
        var items = await _repository.GetAllQuery().ToListAsync();
        
        return items;
    }
}