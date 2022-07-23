using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;

namespace DrugBot.Processors
{
    public class ProcessorHelp : AbstractProcessor
    {
        private readonly IReadOnlyList<AbstractProcessor> _processors;

        private readonly List<string> keys = new List<string>
        {
            "/help",
            "!help",
            "/помощь",
            "!помощь"
        };

        public override string Description => "тутор со всеми командами";
        public override IReadOnlyList<string> Keys => keys;
        public override string Name => "Обучалка";

        public override bool VisiblyDescription => false;

        public ProcessorHelp(IReadOnlyList<AbstractProcessor> processors)
        {
            _processors = processors;
        }

        protected override void OnProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            var answer = string.Join('\n',
                _processors.Where(processor => processor.VisiblyDescription)
                    .Select(processor => $"{processor.Name}\n{processor.Description}\n")
            );
            BotHandler.SendMessage(vkApi, message.PeerId, answer);
        }
    }
}