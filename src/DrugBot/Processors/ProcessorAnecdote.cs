using System.Collections.Generic;
using Anecdotes;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors;

public class ProcessorAnecdote : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/анек",
        "/анекдот",
        "/юмор",
        "/anek",
        "/хех"
    };

    public override string Description => $"Выдать случайный анекдот, для вызова используйте {string.Join(' ', _keys)}";
    public override IReadOnlyList<string> Keys => _keys;
    public override string Name => "Сборник баянов";

    protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
    {
        AnecdoteGenerator generator = sentence.Length > 1
            ? new AnecdoteGenerator(message.Text.GetHashCode())
            : new AnecdoteGenerator();

        BotHandler.SendMessage(vkApi, message.PeerId, generator.GenerateAnecdote().Anecdote, message);
    }
}