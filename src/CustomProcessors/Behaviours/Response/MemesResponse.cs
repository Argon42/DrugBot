using DrugBot.Core.Bot;
using Memes;

namespace CustomProcessors.Behaviours.Response;

public class MemesResponse : Response
{
    public override void ProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
    {
        var generator = message.Text.Split().Length > 1
            ? new MemesGenerator(message.Text.GetHashCode())
            : new MemesGenerator();

        bot.SendMessage(message.CreateResponse(images: new List<byte[]> { generator.GetMeme().Meme }));
    }
}