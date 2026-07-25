using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Dtos;

namespace ToDoList.Api.Controllers;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<long> Register([FromBody] RegisterDto registerDto)
    {
        var userId = await _authService.RegisterAsync(registerDto);
        return userId;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        return result;
    }
}
