using ConsoleApp.Services;
using DrugBot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application = ConsoleApp.Services.Application;


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
    DrugBotServiceConfigurator.ConfigureServices(services);
}

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(ConfigureServices)
    .Build();

var app = host.Services.GetRequiredService<IApplication>();
app.Run();