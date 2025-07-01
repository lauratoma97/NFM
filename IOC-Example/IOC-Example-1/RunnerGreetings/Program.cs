using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Domain;
using Infrastructure.Sqlite;
using Infrastructure.SqlServer;

var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("demo-config.json", optional: false, reloadOnChange: true)
                 .Build().Get<DemoConfig>() ??
             throw new InvalidOperationException("Configuration not found or invalid.");

var services = new ServiceCollection();
services.AddSingleton(config);
services.AddSingleton<BirthdayGreetingService>();

// Register IDbContext based on config
if (config.DatabaseSource == "SqlServer")
{
    services.AddSingleton<IDbContext, SqlServerDbContext>();
}
else if (config.DatabaseSource == "SqlLite")
{
    services.AddSingleton<IDbContext, SqliteDbContext>();
}

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Example usage:
var service = serviceProvider.GetRequiredService<BirthdayGreetingService>();
service.PrintBirthdayGreetings();