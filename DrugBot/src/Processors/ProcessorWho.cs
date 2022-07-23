using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Exception;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorWho : AbstractProcessor
    {
        private readonly List<string> keys = new List<string>
        {
            "/who",
            "/кто"
        };

        public override string Description =>
            $"Бот знает все обо всех, для вызова используйте {string.Join(' ', keys)} вопрос";

        public override IReadOnlyList<string> Keys => keys;

        public override string Name => "Кто?";

        public override bool HasTrigger(Message message, string[] sentence)
        {
            return sentence.Length > 0 &&
                   keys.Any(s => sentence[0].Equals(s, StringComparison.CurrentCultureIgnoreCase));
        }

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            List<string> names;
            try
            {
                names = vkApi.Messages.GetConversationMembers(message.PeerId.Value, new[] { "" })
                    .Profiles
                    .Select(user => $"{user.FirstName} {user.LastName}")
                    .ToList();
            }
            catch (ConversationAccessDeniedException)
            {
                BotHandler.SendMessage(vkApi, message.PeerId,
                    "Для вывода случайного статуса участника, боту необходимы права администратора");
                return;
            }

            var question = message.Text.Substring(sentence[0].Length).TrimStart().TrimEnd("?!".ToCharArray());
            var rnd = new Random(question.ToLower().GetHashCode());
            var result = rnd.Next(0, names.Count());
            var chanceOfNothing = rnd.NextDouble();
            var answer = $"{(chanceOfNothing > 0.9 ? "Никто не" : names[result])} {question}";
            BotHandler.SendMessage(vkApi, message.PeerId, answer);
        }
    }
}