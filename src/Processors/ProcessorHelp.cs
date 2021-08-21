using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public class ProcessorHelp : AbstractProcessor
    {
        private IReadOnlyList<AbstractProcessor> _processors;

        private List<string> keys = new List<string>
        {
            "/help",
            "!help",
            "/помощь",
            "!помощь"
        };

        public ProcessorHelp(IReadOnlyList<AbstractProcessor> processors)
        {
            _processors = processors;
        }

        public override bool VisiblyDescription => false;
        public override string Name => "Обучалка";
        public override IReadOnlyList<string> Keys => keys;
        public override string Description => "тутор со всеми командами";

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            string answer = string.Join('\n',
                _processors.Select(processor => $"{processor.Name}\n{processor.Description}\n"));
            BotHandler.SendMessage(vkApi, message.PeerId, answer);
        }
    }
}