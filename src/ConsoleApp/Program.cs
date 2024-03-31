using DrugBotApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
        {
            string environmentVariable = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentVariable}.json", true)
                .AddEnvironmentVariables()
                .Build();
            services.AddLogging(builder => builder.AddConsole());
            ApplicationConfiguration.ConfigureServices(services, configuration);
        }
    )
    .Build();

await host.StartAsync();