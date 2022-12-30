using System;
using System.Collections.Generic;

namespace EmptyBot;

public class ChatContext
{
    private readonly List<Message> _messages = new();
    private const int MaxMessages = 5;

    public IEnumerable<Message> Messages
    {
        get
        {
            RemoveExpiredMessages();
            return _messages;
        }
    }


    private static readonly TimeSpan MessageLifespan = TimeSpan.FromMinutes(5);

    public void AddMessage(Message message)
    {
        _messages.Add(message);
        EnforceMaxMessageLimit();
    }

    private static bool IsMessageExpired(Message message) =>
        DateTime.Now - message.Time > MessageLifespan;

    private void RemoveExpiredMessages() => _messages.RemoveAll(IsMessageExpired);

    private void EnforceMaxMessageLimit()
    {
        if (_messages.Count > MaxMessages)
            _messages.RemoveRange(0, _messages.Count - MaxMessages);
    }
}
