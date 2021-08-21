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
        private List<string> keys = new List<string>
        {
            "/who",
            "/кто"
        };

        public override string Name => "Бибометр";
        public override IReadOnlyList<string> Keys => keys;

        public override string Description =>
            $"Бот знает все обо всех, для вызова используйте {string.Join(' ', keys)} вопрос";

        public override bool HasTrigger(Message message, string[] sentence) =>
            sentence.Length > 0 &&
            keys.Any(s => sentence[0].Equals(s, StringComparison.CurrentCultureIgnoreCase));

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            List<string> names;
            try
            {
                names = vkApi.Messages.GetConversationMembers(message.PeerId.Value, new[] {""})
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

            string question = message.Text.Substring(sentence[0].Length).TrimStart().TrimEnd("?!".ToCharArray());
            var rnd = new Random(question.ToLower().GetHashCode());
            int result = rnd.Next(0, names.Count());
            double chanceOfNothing = rnd.NextDouble();
            var answer = $"{(chanceOfNothing > 0.9 ? "Никто не" : names[result])} {question}";
            BotHandler.SendMessage(vkApi, message.PeerId, answer);
        }
    }
}