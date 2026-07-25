using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ToDoItemCreateDto dto)
    {
        // JWT Token/Claims orqali kirgan foydalanuvchi ID-sini olish
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                       ?? User.FindFirst("id")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !long.TryParse(userIdClaim, out long userId))
        {
            return Unauthorized("Foydalanuvchi avtorizatsiyadan o'tmagan.");
        }

        var result = await _toDoItemService.CreateAsync(dto, userId);

        return Ok(result);
    }
}
