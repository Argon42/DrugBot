using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DrugBot.Core;
using DrugBot.Core.Bot;

namespace DrugBot.Processors;

public abstract class AbstractProcessor : IProcessor
{
    public abstract string Description { get; }

    public abstract IReadOnlyList<string> Keys { get; }

    public abstract string Name { get; }

    public virtual bool VisiblyDescription => true;

    public virtual bool HasTrigger<TMessage>(TMessage message, string[] sentence) where TMessage : IMessage
    {
        if (sentence.Length > 1 && BotHandler.IsBotTrigger(sentence[0]))
            return CheckTrigger(sentence[1]);
        if (sentence.Length >= 1)
            return CheckTrigger(sentence[0]);

        return false;
    }

    public bool TryProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message, CancellationToken token)
        where TUser : IUser
        where TMessage : IMessage<TMessage, TUser>
    {
        try
        {
            OnProcessMessage(bot, message, token);
        }
        catch (Exception e)
        {
            // _logger.LogError(e, $"Error on message processing");
            OnProcessMessageError(bot, message, e);
            return false;
        }

        return true;
    }

    protected virtual void OnProcessMessage<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        CancellationToken token)
        where TUser : IUser
        where TMessage : IMessage<TMessage, TUser>
    {
    }

    protected virtual void OnProcessMessageError<TUser, TMessage>(IBot<TUser, TMessage> bot, TMessage message,
        Exception exception)
        where TUser : IUser
        where TMessage : IMessage<TMessage, TUser>
    {
    }

    private bool CheckTrigger(string sentence)
    {
        return Keys.Any(s => sentence.Equals(s, StringComparison.CurrentCultureIgnoreCase));
    }
}