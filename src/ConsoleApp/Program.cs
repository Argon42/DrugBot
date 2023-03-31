using ConsoleApp.Services;
using DrugBot;
using DrugBot.Bot.Vk;
using DrugBot.Common;
using DrugBot.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VkNet.Abstractions;
using VkNet.Model;
using Application = ConsoleApp.Services.Application;


void SetupVk(IServiceCollection services)
{
    services.AddSingleton<IVkBotHandler, VkBotHandler>();
    services.AddSingleton<IVkBot, VkBot>();
    services.AddSingleton<IFactory<IVkApi>, VkFactory.Api>();
    services.AddSingleton<IFactory<LongPollServerResponse>, VkFactory.LongPollServer>();
    services.AddSingleton<IFactory<VkConfigs>, VkFactory.Config>();
    services.AddFromFactory<VkConfigs, IFactory<VkConfigs>>();
}

void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IApplication, Application>();
    services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
            optional: true)
        .AddEnvironmentVariables()
        .Build());
    SetupVk(services);
    services.AddSingleton<BotHandler, BotHandler>();
    services.AddProcessors();
}

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(ConfigureServices)
    .Build();

var app = host.Services.GetRequiredService<IApplication>();
app.Run();


static class DiExtension
{
    public static void AddProcessors(this IServiceCollection services)
    {
        var typesWithMyAttribute =
            from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            let attributes = t.GetCustomAttributes(typeof(ProcessorAttribute), true)
            where attributes is { Length: > 0 }
            select new { Type = t, Attributes = attributes.Cast<ProcessorAttribute>() };
        foreach (var x1 in typesWithMyAttribute)
        {
            if (x1.Type.BaseType == typeof(AbstractProcessor))
                services.AddScoped(typeof(AbstractProcessor), x1.Type);
            services.AddScoped(x1.Type);
        }
    }

    public static void AddFromFactory<T, TFactory>(this IServiceCollection services)
        where TFactory : IFactory<T>
        where T : class
    {
        services.AddScoped<T>(provider => provider.GetRequiredService<TFactory>().Create());
    }
}