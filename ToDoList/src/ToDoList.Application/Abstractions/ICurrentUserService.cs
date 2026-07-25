using ToDoList.Domain.Entities;

namespace ToDoList.Application.Abstractions;

public interface ICurrentUserService
{
    long? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    UserRole? Role { get; }
    bool IsAuthenticated { get; }
}
