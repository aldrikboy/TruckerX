using System;
using System.Collections.Generic;
using System.Text;

namespace TruckerX.Messaging
{
    public enum MessageType
    {
        Error,
        Warning,
        Info,
    }

    public class Message
    {
        public string[] Words { get; }
        public MessageType Type { get; }

        public Message(string[] text, MessageType type)
        {
            Words = text;
            Type = type;
        }
    }

    public static class MessageLog
    {
        public static List<Message> Messages { get; } = new List<Message>();

        public static void Add(string text, MessageType type)
        {
            Messages.Add(new Message(text.Split(" ", StringSplitOptions.RemoveEmptyEntries), type));
        }

        public static void AddError(string text)
        {
            Add(text, MessageType.Error);
        }

        public static void AddInfo(string text)
        {
            Add(text, MessageType.Info);
        }

        public static void AddWarning(string text)
        {
            Add(text, MessageType.Warning);
        }
    }
}
