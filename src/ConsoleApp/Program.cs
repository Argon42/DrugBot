using DrugBotApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(ApplicationConfiguration.ConfigureServices)
    .Build();

IApplication app = host.Services.GetRequiredService<IApplication>();
app.Run();