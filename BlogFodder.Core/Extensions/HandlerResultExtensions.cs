using BlogFodder.Core.Shared.Models;

namespace BlogFodder.Core.Extensions;

public static class HandlerResultExtensions
{

    /// <summary>
    /// Return all error messages in a list string
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static IEnumerable<string?> ErrorMessagesToList(this List<HandlerResultMessage> messages)
    {
        return messages.ErrorMessages().Select(x => x.Message);
    }

    /// <summary>
    /// Gets only the error messages
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static IEnumerable<HandlerResultMessage> ErrorMessages(this List<HandlerResultMessage> messages)
    {
        return messages.Where(x => x.MessageType == HandlerResultMessageType.Error);
    }
    
    /// <summary>
    /// Gets only the warning messages
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static IEnumerable<HandlerResultMessage> WarningMessages(this List<HandlerResultMessage> messages)
    {
        return messages.Where(x => x.MessageType == HandlerResultMessageType.Warning);
    }
    
    /// <summary>
    /// Adds a handler result message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="handlerResult"></param>
    /// <param name="message"></param>
    /// <param name="handlerResultMessageType"></param>
    public static void AddMessage<T>(this HandlerResult<T> handlerResult, string message, HandlerResultMessageType handlerResultMessageType)
    {
        handlerResult.Messages.Add(new HandlerResultMessage
        {
            Message = message,
            MessageType = handlerResultMessageType
        });
    }

    /// <summary>
    /// Adds many messages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="handlerResult"></param>
    /// <param name="messages"></param>
    /// <param name="handlerResultMessageType"></param>
    public static void AddMessage<T>(this HandlerResult<T> handlerResult, List<string> messages, HandlerResultMessageType handlerResultMessageType)
    {
        foreach (var m in messages)
        {
            handlerResult.AddMessage(m, handlerResultMessageType);
        }
    }

    /// <summary>
    /// Adds many messages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="handlerResult"></param>
    /// <param name="messages"></param>
    /// <param name="handlerResultMessageType"></param>
    public static void AddMessage<T>(this HandlerResult<T> handlerResult, IEnumerable<string> messages, HandlerResultMessageType handlerResultMessageType)
    {
        handlerResult.AddMessage(messages.ToList(), handlerResultMessageType);
    }
}