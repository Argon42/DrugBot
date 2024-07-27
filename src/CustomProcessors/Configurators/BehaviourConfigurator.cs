using CustomProcessors.Behaviours.Response;
using CustomProcessors.Behaviours.Triggers;
using Microsoft.Extensions.DependencyInjection;

namespace CustomProcessors.Configurators;

public static class BehaviourConfigurator
{
    public static void ConfigureBehaviours(IServiceCollection services)
    {
        ConfigureResponses(services);
        ConfigureTriggers(services);
        ConfigureOthers(services);
    }

    private static void ConfigureResponses(IServiceCollection services)
    {
        services.AddTransient<SendResponseMessage>();
        services.AddTransient<RandomResponseMessage>();
        services.AddTransient<AnecdoteResponse>();
        services.AddTransient<BibaResponse>();
        services.AddTransient<BibasiksResponse>();
        services.AddTransient<DeadChineseResponse>();
        services.AddTransient<DiceResponse>();
        services.AddTransient<DiplomaResponse>();
        services.AddTransient<TotemResponse>();
        services.AddTransient<TryResponse>();
        services.AddTransient<WisdomResponse>();
    }

    private static void ConfigureTriggers(IServiceCollection services)
    {
        services.AddTransient<MessageText>();
        services.AddTransient<MessageTextStartFrom>();
    }

    private static void ConfigureOthers(IServiceCollection services)
    {
    }
}