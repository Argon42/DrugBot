using System.Collections.Generic;
using DrugBot.Bot;
using DrugBot.Core.Bot;
using DrugBot.Core.Common;
using Memes;

namespace DrugBot.Processors;

[Processor]
public class ProcessorMemes : AbstractProcessor
{
    private readonly List<string> _keys = new()
    {
        "/мем",
        "/mem",
        "/memes",
        "/мемы",
        "/кек",
    };

    public override string Description => $"Выдать случайный мем, для вызова используйте {string.Join(' ', _keys)}";
    public override IReadOnlyList<string> Keys => _keys;
    public override string Name => "Сборник баянов";

    protected override void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message)
    {
        MemesGenerator generator = message.Text.Split().Length > 1
            ? new MemesGenerator(message.Text.GetHashCode())
            : new MemesGenerator();

        bot.SendMessage(message.CreateResponse(images: new List<byte[]> { generator.GetMeme().Meme }));
    }
}