using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Dtos;
using ToDoList.Application.Services;

namespace ToDoList.Api.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ICurrentUserService _currentUserService;

    public AuthController(IAuthService authService, ICurrentUserService currentUserService)
    {
        _authService = authService;
        _currentUserService = currentUserService;
    }

    [HttpPost("register")]
    public async Task<long> Register([FromBody] RegisterDto registerDto)
    {
        var userId = await _authService.RegisterAsync(registerDto);
        return userId;
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<CurrentUserDto> Me()
    {
        return Ok(new CurrentUserDto
        {
            UserId = _currentUserService.UserId,
            UserName = _currentUserService.UserName,
            Email = _currentUserService.Email,
            Role = _currentUserService.Role?.ToString(),
            IsAuthenticated = _currentUserService.IsAuthenticated
        });
    }
}
