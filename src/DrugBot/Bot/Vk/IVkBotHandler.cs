using System;
using System.Threading.Tasks;

namespace DrugBot.Bot.Vk;

public interface IVkBotHandler : IDisposable
{
    bool Enabled { get; }
    void Initialize();
    Task Start();
    void Stop();
}