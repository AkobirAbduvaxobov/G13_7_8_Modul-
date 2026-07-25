using ToDoList.Api.Services;
using ToDoList.Application.Abstractions;

namespace ToDoList.Api.Configurations;

public static class CurrentUserConfiguration
{
    public static void ConfigureCurrentUser(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
    }
}
