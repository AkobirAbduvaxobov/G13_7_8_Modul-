using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Entities;
using ToDoList.Infrastructure.Persistence;

namespace ToDoList.Api.Controllers;

[Route("api/v1/todoitems")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ToDoItemsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var item = await _context.ToDoItems.FirstOrDefaultAsync(x => x.ToDoItemId == id && !x.IsDeleted);

        if (item is null)
        {
            return NotFound(new { message = "ToDo item topilmadi." });
        }

        item.IsDeleted = true;
        item.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}