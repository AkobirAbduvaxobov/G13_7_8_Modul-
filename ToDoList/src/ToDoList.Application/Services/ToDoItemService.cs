using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Abstractions;
using ToDoList.Infrastructure.Persistence;

namespace ToDoList.Application.Services;

public class ToDoItemService : IToDoItemService
{
    private readonly AppDbContext _context;

    public ToDoItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var item = await _context.ToDoItems
            .FirstOrDefaultAsync(x => x.ToDoItemId == id && !x.IsDeleted, cancellationToken);

        if (item is null)
        {
            throw new KeyNotFoundException($"Id: {id} bo'lgan ToDoItem topilmadi.");
        }

        // Soft delete
        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}-