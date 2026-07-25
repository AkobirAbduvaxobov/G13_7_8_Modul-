using ToDoList.Application.Dtos;

namespace ToDoList.Application.Abstractions;

public interface IAuthService
{
    Task<long> RegisterAsync(RegisterDto registerDto);
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
}
