// See https://aka.ms/new-console-template for more information

using DrugBot;

Console.WriteLine("StartBot");

var tokenSource = new CancellationTokenSource();
await new Bot().Start(tokenSource.Token);