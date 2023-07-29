using System;
using System.Linq;
using DrugBot.Core;
using DrugBot.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DrugBot;

public class DrugBotServiceConfigurator
{
    public static void ConfigureServices(IServiceCollection services, ILogger<DrugBotServiceConfigurator> logger)
    {
        AddProcessors(services, logger);
        services.AddSingleton<BotHandler, BotHandler>();
    }

    private static void AddProcessors(IServiceCollection services, ILogger<DrugBotServiceConfigurator> logger)
    {
        var typesWithMyAttribute = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes(), (assembly, type) => new { assembly, type })
            .Select(pair => new { pair, attributes = pair.type.GetCustomAttributes(typeof(ProcessorAttribute), true) })
            .Where(pair => pair.attributes is { Length: > 0 })
            .Select(pair => new { Type = pair.pair.type, Attributes = pair.attributes.Cast<ProcessorAttribute>() });
        foreach (var x1 in typesWithMyAttribute)
        {
            Type? interfaceOfProcessor = x1.Type.GetInterface(nameof(IProcessor));
            if (interfaceOfProcessor != default)
            {
                services.AddSingleton(typeof(IProcessor), x1.Type);
                logger.LogInformation("IProcessor added, type: {Type}", x1.Type);
            }
        }
    }
}