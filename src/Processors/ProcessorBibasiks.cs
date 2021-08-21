using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorBibasiks : AbstractProcessor
    {
        private List<string> keys = new List<string>
        {
            "/бибасики"
        };

        public override string Name => "Бибасикометр";
        public override IReadOnlyList<string> Keys => keys;

        public override string Description =>
            $"Узнай размеры своих бибасиков, для вызова используйте {string.Join(' ', keys)}";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            try
            {
                if (message.FromId != null &&
                    vkApi.Users.Get(new[] {message.FromId.Value}, ProfileFields.Sex)[0].Sex ==
                    Sex.Male)
                {
                    BotHandler.SendMessage(vkApi, message.PeerId, "У вас нет бибасиков у вас биба");
                    return;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            var rnd = new Random(BotHandler.GetDayUserSeed(message.FromId));
            int result = rnd.Next(20, 150);
            BotHandler.SendMessage(vkApi, message.PeerId, $"Сегодня ваши бибасики {result} см в обхвате");
        }
    }
}