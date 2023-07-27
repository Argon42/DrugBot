using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DrugBot.Core.Common;

public static class DiExtension
{
    [PublicAPI]
    public static void AddScopeFromFactory<T, TFactory>(this IServiceCollection services)
        where TFactory : IFactory<T> where T : class
    {
        services.AddScoped(CreateFromFabric<T, TFactory>);
    }

    [PublicAPI]
    public static void AddSingletonFromFactory<T, TFactory>(this IServiceCollection services)
        where TFactory : IFactory<T> where T : class
    {
        services.AddSingleton(CreateFromFabric<T, TFactory>);
    }

    [PublicAPI]
    public static void AddTransientFromFactory<T, TFactory>(this IServiceCollection services)
        where TFactory : IFactory<T> where T : class
    {
        services.AddTransient(CreateFromFabric<T, TFactory>);
    }

    [PublicAPI]
    private static T CreateFromFabric<T, TFactory>(IServiceProvider provider) 
        where TFactory : IFactory<T> where T : class
    {
        return provider.GetRequiredService<TFactory>().Create();
    }
}