using CustomProcessors.Configurators;
using DrugBot.Core;
using DrugBot.Vk;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DrugBot.Infrastructure;

public static class ApplicationConfiguration
{
    public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddHostedService<Application>();
        services.AddSingleton<IBotsHandlerController, BotsHandlerController>();
        services.AddSingleton(configuration);

        var loggerBot = services.BuildServiceProvider().GetRequiredService<ILogger<DrugBotServiceConfigurator>>();
        DrugBotServiceConfigurator.ConfigureServices(services, loggerBot);

        var loggerCustomProcessors = services.BuildServiceProvider().GetRequiredService<ILogger<CustomProcessorsServiceConfigurator>>();
        CustomProcessorsServiceConfigurator.ConfigureServices(services, configuration, loggerCustomProcessors);
        VkServiceConfigurator.ConfigureVk(services);
    }
}