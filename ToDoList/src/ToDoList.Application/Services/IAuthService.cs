using ToDoList.Application.Dtos;

namespace ToDoList.Application.Services;

public interface IAuthService
{
    Task<long> RegisterAsync(RegisterDto registerDto);
}