using DrugBot.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DrugBot;

public static class DiExtension
{
    public static void AddFromFactory<T, TFactory>(this IServiceCollection services)
        where TFactory : IFactory<T>
        where T : class
    {
        services.AddScoped<T>(provider => provider.GetRequiredService<TFactory>().Create());
    }
}