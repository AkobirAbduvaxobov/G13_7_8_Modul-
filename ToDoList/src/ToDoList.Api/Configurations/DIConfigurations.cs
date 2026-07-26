using ToDoList.Api.Services;
using ToDoList.Application.Abstractions;

namespace ToDoList.Api.Configurations;

public static class DIConfigurations
{
    public static void ConfigureDI(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
    }
}
