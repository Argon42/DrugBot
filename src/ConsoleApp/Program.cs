using DrugBot;

Console.WriteLine("StartBot");
const string appId = "VK_APP_ID";
const string groupToken = "VK_GROUP_TOKEN";

string? token = Environment.GetEnvironmentVariable(groupToken);
string? id = Environment.GetEnvironmentVariable(appId);

if (token == null || id == null)
    throw new Exception($"Environment variables {appId} and {groupToken} not exist");

if (uint.TryParse(id, out uint parsedId) == false)
    throw new Exception($"Environment variable {appId} is incorrect");

Configs environmentConfig = new() { Token = token ?? "", Id = parsedId };


CancellationTokenSource tokenSource = new();
await new Bot().Start(environmentConfig, tokenSource.Token);