using System;
using VkNet;
using VkNet.Model;

namespace BananvaBot
{
    public abstract class AbstractProcessor
    {
        public abstract bool HasTrigger(Message message, string[] sentence);

        public bool TryProcessMessage(VkApi vkApi, Message message, string[] sentence)
        {
            if (HasTrigger(message, sentence) == false) return false;

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