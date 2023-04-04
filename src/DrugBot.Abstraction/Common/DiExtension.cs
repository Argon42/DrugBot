using Microsoft.Extensions.DependencyInjection;

namespace DrugBot.Core.Common;

public static class DiExtension
{
    public static void AddScopeFromFactory<T, TFactory>(this IServiceCollection services)
        where TFactory : IFactory<T> where T : class
    {
        services.AddScoped(CreateFromFabric<T, TFactory>);
    }
    
    public static void AddSingletonFromFactory<T, TFactory>(this IServiceCollection services)
        where TFactory : IFactory<T> where T : class
    {
        services.AddSingleton(CreateFromFabric<T, TFactory>);
    }
    
    public static void AddTransientFromFactory<T, TFactory>(this IServiceCollection services)
        where TFactory : IFactory<T> where T : class
    {
        services.AddTransient(CreateFromFabric<T, TFactory>);
    }

    private static T CreateFromFabric<T, TFactory>(IServiceProvider provider) 
        where TFactory : IFactory<T> where T : class
    {
        return provider.GetRequiredService<TFactory>().Create();
    }
}