using System.Text;
using DrugBot.Vk.Bot;
using DrugBot.Core.Bot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.Abstractions;

namespace DrugBot.Vk;

public static class VkServiceConfigurator
{
    public static void ConfigureVk(IServiceCollection services)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        services
            .AddSingleton<IBotHandler, VkBot>()
            .AddSingleton<IVkApi, VkApi>(provider => new VkApi(provider.GetRequiredService<ILogger<VkApi>>()))
            .AddTransient<VkBotConfiguration>();
    }
}