using System;

namespace EmptyBot;

public struct Message
{
    public string Text { get; set; }
    public string SenderName { get; set; }
    public DateTimeOffset Time { get; set; }

    public Message(string text, string senderName, DateTimeOffset time)
    {
        Text = text;
        SenderName = senderName;
        Time = time;
    }

    public override string ToString()
    {
        return $"{SenderName}: {Text}";
    }
}
