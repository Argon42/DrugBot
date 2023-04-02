using DrugBot;
using DrugBotApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IApplication, Application>();
    services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
            true)
        .AddEnvironmentVariables()
        .Build());
    DrugBotServiceConfigurator.ConfigureServices(services);
}

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(ConfigureServices)
    .Build();

IApplication app = host.Services.GetRequiredService<IApplication>();
app.Run();