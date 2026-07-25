using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Services;
using ToDoList.Application.Dtos;
using Microsoft.AspNetCore.Http; // Для StatusCodes

namespace ToDoList.Api.Controllers;

[Route("api/v1/todoitems")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly IToDoItemService _toDoItemService;

    public ToDoItemsController(IToDoItemService toDoItemService)
    {
        _toDoItemService = toDoItemService;
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(List<ToDoItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string searchTerm)
    {
        var result = await _toDoItemService.Search(searchTerm);
        return Ok(result);
    }
}