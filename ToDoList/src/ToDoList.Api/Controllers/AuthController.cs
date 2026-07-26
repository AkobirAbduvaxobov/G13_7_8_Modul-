using Microsoft.AspNetCore.Authorization;
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
    public async Task<LoginResponseDto> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);
        return result;
    }

    [HttpPost("refresh-token")]
    public async Task<LoginResponseDto> RefreshToken(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        var token = await _authService.RefreshTokenAsync(refreshTokenRequestDto);
        return token;
    }

    [HttpPost("logout")]
    public async Task Logout(RefreshTokenRequestDto refreshTokenRequestDto)
    {
        await _authService.LogoutAsync(refreshTokenRequestDto);
    }
}
