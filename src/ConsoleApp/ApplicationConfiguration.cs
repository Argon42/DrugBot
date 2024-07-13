using Anecdotes.CommunityAnecdotes.Data;
using Anecdotes.CommunityAnecdotes.Repositories;
using Anecdotes.CommunityAnecdotes.Repositories.Interfaces;
using CustomProcessors;
using DrugBot;
using DrugBot.Vk;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DrugBotApp;

public static class ApplicationConfiguration
{
    public static void ConfigureServices(IServiceCollection services)
    {
        IConfiguration configuration = ConfigureConfiguration();
        
        services.AddSingleton<IApplication, Application>();
        services.AddSingleton<IApplicationInput, ApplicationInput>();
        services.AddSingleton(configuration);
        
        services.AddLogging(builder => builder.AddConsole());
        
        ConfigureDb(services, configuration);

        var loggerBot = services.BuildServiceProvider().GetRequiredService<ILogger<DrugBotServiceConfigurator>>();
        DrugBotServiceConfigurator.ConfigureServices(services, loggerBot);
        
        var loggerCustomProcessors = services.BuildServiceProvider().GetRequiredService<ILogger<CustomProcessorsServiceConfigurator>>();
        CustomProcessorsServiceConfigurator.ConfigureServices(services, configuration, loggerCustomProcessors);
        VkServiceConfigurator.ConfigureVk(services);
    }

    private static void ConfigureDb(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<CommunityAnecdoteDbContext>(options => options.UseNpgsql(connectionString));
        services.AddSingleton<IAnecdoteRepository, CommunityAnecdoteRepository>();
    }

    private static IConfigurationRoot ConfigureConfiguration() =>
        new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true)
            .AddEnvironmentVariables()
            .Build();
}