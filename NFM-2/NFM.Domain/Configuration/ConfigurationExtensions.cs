using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NFM.Domain.Configuration;

public static class ConfigurationExtensions
{
    public static void AddAppSettings(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var appSettings = new AppSettings();
        configuration.Bind(appSettings);
        serviceCollection.AddSingleton<IAppSettings>(appSettings);
    }
}