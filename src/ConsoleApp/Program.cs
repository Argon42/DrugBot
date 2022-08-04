using DrugBot;

Console.WriteLine("StartBot");

string? token = Environment.GetEnvironmentVariable("VK_GROUP_TOKEN");
string? id = Environment.GetEnvironmentVariable("VK_APP_ID");
bool isEnvironmentSettings = token != null && id != null;

Configs environmentConfig = new() { Token = token ?? "", Id = uint.Parse(id ?? "0") };
Configs configs = isEnvironmentSettings ? environmentConfig : Configs.GetConfig();

CancellationTokenSource tokenSource = new();
await new Bot().Start(configs, tokenSource.Token);