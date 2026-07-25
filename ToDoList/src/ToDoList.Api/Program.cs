
using ToDoList.Api.Configurations;
using ToDoList.Infrastructure;
using ToDoList.Application;
using ToDoList.Application.Services;
namespace ToDoList.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

        builder.ConfigureJwt();
        builder.ConfigureDBConnectionString();
        builder.Services.ConfigureInfrastructure(builder.Configuration);
        builder.Services.ConfigureApplication(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (true || app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
