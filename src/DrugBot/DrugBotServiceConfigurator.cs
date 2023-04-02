using System;
using System.Linq;
using DrugBot.Bot.Vk;
using DrugBot.Core;
using DrugBot.Core.Common;
using DrugBot.Processors;
using Microsoft.Extensions.DependencyInjection;
using VkNet.Abstractions;
using VkNet.Model;

namespace DrugBot;

public static class DrugBotServiceConfigurator
{
    public static void ConfigureServices(IServiceCollection services)
    {
        ConfigureVk(services);
        AddProcessors(services);
        services.AddSingleton<BotHandler, BotHandler>();
    }

    private static void AddProcessors(IServiceCollection services)
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
                services.AddScoped(typeof(IProcessor), x1.Type);
            services.AddScoped(x1.Type);
        }
    }

    private static void ConfigureVk(IServiceCollection services)
    {
        services.AddSingleton<IVkBotHandler, VkBotHandler>();
        services.AddSingleton<IVkBot, VkBot>();
        services.AddSingleton<IFactory<IVkApi>, VkFactory.Api>();
        services.AddSingleton<IFactory<LongPollServerResponse>, VkFactory.LongPollServer>();
        services.AddSingleton<IFactory<VkConfigs>, VkFactory.Config>();
        services.AddFromFactory<VkConfigs, IFactory<VkConfigs>>();
    }
}