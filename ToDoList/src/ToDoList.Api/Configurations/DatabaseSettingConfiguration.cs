using ToDoList.Api.Settings;

namespace ToDoList.Api.Configurations;

public static class DatabaseSettingConfiguration
{
    public static void ConfigureDBConnectionString(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

        var dBConnectionString = new DatabaseSettings
        {
            ConnectionString = connectionString
        };

        builder.Services.AddSingleton(dBConnectionString);
    }
}
