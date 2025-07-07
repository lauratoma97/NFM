using Microsoft.IdentityModel.Tokens;
using System.Text;
using Centric.Finance.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using NFM.Domain.Context;
using NFM.Domain.Models;

namespace NFM.WebApi.Authentication;

public static class DiConfig
{
    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration, Serilog.ILogger logger)
    {
        var jwtAppSettingOptions = configuration.GetSection("JwtOptions").Get<JwtOptionsSettings>();
        var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingOptions.SigningKey));

        // configure database link to an IdentityUser -> MyApplicationUser using Entity Framework
        services.AddIdentityCore<MyApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<MyApplicationUser>>("nfm-provider")
            .AddEntityFrameworkStores<MyDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // NOTE: IOptions<IdentityOptions> is available for dependency injection, and it's being used inside the Identity framework or other frameworks that depend on options
            // configure our own preferred policies
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtAppSettingOptions.Issuer,
                    ValidAudience = jwtAppSettingOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtAppSettingOptions.SigningKey))
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.IncludeErrorDetails = true;

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        logger.Warning($"Failed authentication attempt: {context.Exception.Message} - {context.Request.Headers["Authorization"]}");
                        return Task.CompletedTask;
                    }
                };
            });

        // NOTE: IOptions<JwtOptions> is available for dependency injection (Eg: used inside AuthController)
        // they inject configured values necessary for JWT token generation
        services.Configure<JwtOptions>(options =>
        {
            options.Issuer = jwtAppSettingOptions.Issuer;
            options.Audience = jwtAppSettingOptions.Audience;
            options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            if (int.TryParse(jwtAppSettingOptions.ValidForSeconds, out var validFor))
            {
                options.ValidFor = new TimeSpan(0, 0, validFor);
            }
        });
    }
}