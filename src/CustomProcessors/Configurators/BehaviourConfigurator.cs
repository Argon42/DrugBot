using CustomProcessors.Behaviours.Response;
using CustomProcessors.Behaviours.Triggers;
using Microsoft.Extensions.DependencyInjection;

namespace CustomProcessors;

public static class BehaviourConfigurator
{
    public static void ConfigureBehaviours(IServiceCollection services)
    {
        ConfigureResponses(services);
        ConfigureTriggers(services);
    }

    private static void ConfigureResponses(IServiceCollection services)
    {
        services.AddTransient<SendResponseMessage>();
    }

    private static void ConfigureTriggers(IServiceCollection services)
    {
        services.AddTransient<MessageText>();
    }
}