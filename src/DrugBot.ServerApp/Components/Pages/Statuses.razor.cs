using DrugBot.Core.Bot;

namespace DrugBot.ServerApp.Components.Pages;

public partial class Statuses : IDisposable, IAsyncDisposable
{
    private IEnumerable<BotStatus> _bots = ArraySegment<BotStatus>.Empty;
    private Timer? _timer;

    protected override async Task OnInitializedAsync()
    {
        await UpdateBotList();
        _timer = new Timer(
            _ => UpdateBotList().GetAwaiter().GetResult(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(30));
    }

    private void Start(BotStatus bot)
    {
        BotsHandlerController.StartHandler(bot.Name);
    }

    private async Task UpdateBotList()
    {
        _bots = BotsHandlerController.GetStatus();
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_timer != null) await _timer.DisposeAsync();
    }
}