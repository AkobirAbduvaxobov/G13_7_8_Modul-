using System.Security.Claims;
using ToDoList.Application.Abstractions;
using ToDoList.Domain.Entities;

namespace ToDoList.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public long? UserId
        => long.TryParse(FindFirst(ClaimTypes.NameIdentifier, "sub"), out var id) ? id : null;

    public string? UserName => FindFirst(ClaimTypes.Name, "unique_name", "name");

    public string? Email => FindFirst(ClaimTypes.Email, "email");

    public UserRole? Role
        => Enum.TryParse<UserRole>(FindFirst(ClaimTypes.Role, "role"), ignoreCase: true, out var role)
            ? role
            : null;

    private string? FindFirst(params string[] claimTypes)
    {
        var user = User;
        if (user is null)
            return null;

        foreach (var type in claimTypes)
        {
            var value = user.FindFirst(type)?.Value;
            if (!string.IsNullOrEmpty(value))
                return value;
        }

        return null;
    }
}
