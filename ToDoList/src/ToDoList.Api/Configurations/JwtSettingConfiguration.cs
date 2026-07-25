using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Api.Settings;

namespace ToDoList.Api.Configurations;

public static class JwtSettingConfiguration
{
    public static void ConfigureJwt(this WebApplicationBuilder builder)
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var secretKey = builder.Configuration["Jwt:SecurityKey"];
        var lifetime = builder.Configuration["Jwt:Lifetime"];
        var refreshTokenLifetimeDays = builder.Configuration["Jwt:RefreshTokenLifetimeDays"];

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("Jwt:SecurityKey is not configured.");

        var jwtSettings = new JwtSettings
        {
            Issuer = issuer,
            Audience = audience,
            SecretKey = secretKey,
            Lifetime = int.Parse(lifetime),
            RefreshTokenLifetimeDays = int.Parse(refreshTokenLifetimeDays)
        };

        builder.Services.AddSingleton(jwtSettings);

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,

                    ValidateAudience = true,
                    ValidAudience = audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}
