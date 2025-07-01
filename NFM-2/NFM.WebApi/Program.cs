using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NFM.Business.Validators;
using NFM.Business.ModelDTOs;
using NFM.Business.Services.Contracts;
using NFM.Business.Services.Implementations;
using NFM.Domain.Repositories;
using NFM.Business.ModelMapping;
using NFM.Domain.Context;
using NFM.WebApi.Authentication;
using Serilog;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("C:\\DemoLogs\\log.txt", rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Services.ConfigureJwtAuthentication(builder.Configuration, logger);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NFM API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IValidator<ProductDto>, ProductValidator>();
builder.Services.AddScoped<IValidator<CreateProductDto>, CreateProductValidator>();
builder.Services.AddScoped<IValidator<EmployeeDto>, EmployeeValidator>();

builder.Services.AddScoped<AppSeeder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<AppSeeder>();
    await dataSeeder.SeedAsync();
}

app.Run();
