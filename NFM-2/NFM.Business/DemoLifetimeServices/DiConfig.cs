using Microsoft.Extensions.DependencyInjection;

namespace NFM.Business.DemoLifetimeServices;

public static class DiConfig
{
    public static void AddMyDemoLifetime(this IServiceCollection services)
    {
        // singleton - one instance for the whole application created once per application lifetime
        services.AddSingleton<ServerConnectionDb>();

        // scoped - one instance per request, created once per request lifetime 
        // question: what happens if we inject a scoped service into a singleton?
        // an usercase?
        services.AddScoped<CurrentRequest>();

        // transient - a new instance is created every time it is requested
        // question: what happens if we inject a transient service into a singleton?
        services.AddTransient<TransientB>();
        services.AddTransient<TransientA>();
    }
}