using ToDoList.Api.Configurations;
using ToDoList.Application;
using ToDoList.Infrastructure;

namespace ToDoList.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.ConfigureSwagger();
        builder.ConfigureJwt();
        builder.ConfigureDBConnectionString();
        builder.Services.ConfigureInfrastructure(builder.Configuration);
        builder.Services.ConfigureApplication(builder.Configuration);

        var app = builder.Build();

        if (true || app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
