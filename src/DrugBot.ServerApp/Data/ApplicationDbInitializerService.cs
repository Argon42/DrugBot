namespace DrugBot.ServerApp.Data;

public class ApplicationDbInitializerService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationDbInitializerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>();
            await initializer.Initialize();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}