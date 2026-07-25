using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services;

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

    [HttpGet("statistics")]
    public async Task<ToDoItemStatisticsDto> GetStatistics()
    {
        return await _toDoItemService.GetStatisticsAsync();
    }
}
