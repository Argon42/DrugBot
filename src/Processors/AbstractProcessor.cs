using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public abstract class AbstractProcessor
    {
        public virtual bool VisiblyDescription => true;
        public abstract string Name { get; }
        public abstract IReadOnlyList<string> Keys { get; }
        public abstract string Description { get; }

        private bool CheckTrigger(string sentence) =>
            Keys.Any(s => sentence.Equals(s, StringComparison.CurrentCultureIgnoreCase));

        public virtual bool HasTrigger(Message message, string[] sentence)
        {
            if (sentence.Length > 1 && BotHandler.IsBotTrigger(sentence[0]))
                return CheckTrigger(sentence[1]);
            if (sentence.Length >= 1)
                return CheckTrigger(sentence[0]);

            return false;
        }

        public bool TryProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            if (HasTrigger(message, sentence) == false)
                return false;

            try
            {
                OnProcessMessage(vkApi, message, sentence);
            }
            catch (Exception e)
            {
                OnProcessMessageError(vkApi, message, sentence, e);
                return false;
            }

            return true;
        }

        protected abstract void OnProcessMessage(VkApi vkApi, Message message, string[] sentence);

        protected virtual void OnProcessMessageError(VkApi vkApi, Message message, string[] sentence,
            Exception exception)
        {
        }
    }
}