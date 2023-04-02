using System;
using System.Threading.Tasks;

namespace ConsoleApp.Services;

public interface IVkBotHandler : IDisposable
{
    void Initialize();
    Task Start();
    void Stop();
    bool Enabled { get; }
}