using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Application.Abstractions;
using ToDoList.Application.Services;
using ToDoList.Infrastructure.Persistence;
using ToDoList.Infrastructure.Persistence.Implementations;

namespace ToDoList.Infrastructure;

public static class Configurations
{
    public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DatabaseConnection");

        services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(connectionString));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
    }
}
