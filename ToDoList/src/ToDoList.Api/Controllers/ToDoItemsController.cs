using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Services;

namespace ToDoList.Api.Controllers;

[Route("api/v1/todoitems")]
[ApiController]
public class ToDoItemsController : ControllerBase
{
    private readonly IToDoItemService _todoItemService;

    public ToDoItemsController(IToDoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _todoItemService.GetAllAsync();
        return Ok(items);
    }
}