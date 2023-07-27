﻿using System;
using System.Linq;
using System.Text;
using DrugBot.Bot.Vk;
using DrugBot.Core;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using VkNet.Abstractions;
using VkNet.Model;

namespace DrugBot;

public static class DrugBotServiceConfigurator
{
    public static void ConfigureServices(IServiceCollection services)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
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
        }
    }

    private static void ConfigureVk(IServiceCollection services)
    {
        services.AddSingleton<IBotHandler, VkBot>();
        services.AddSingleton<IFactory<IVkApi>, VkFactory.Api>();
        services.AddSingleton<IFactory<LongPollServerResponse>, VkFactory.LongPollServer>();
        // TODO: change to Binder with services.Add(vkConfig) from configuration.Get<VkConfig>() 
        services.AddSingleton<IFactory<VkConfigs>, VkFactory.Config>();
        services.AddScopeFromFactory<VkConfigs, IFactory<VkConfigs>>();
    }
}