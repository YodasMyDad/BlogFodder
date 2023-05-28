using BlogFodder.Core.Membership.Models;
using BlogFodder.Core.Shared.Models;

namespace BlogFodder.Core.Extensions;

public static class ResultExtensions
{

    /// <summary>
    /// Return all error messages in a list string
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static IEnumerable<string?> ErrorMessagesToList(this List<ResultMessage> messages)
    {
        return messages.ErrorMessages().Select(x => x.Message);
    }

    /// <summary>
    /// Gets only the error messages
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static IEnumerable<ResultMessage> ErrorMessages(this List<ResultMessage> messages)
    {
        return messages.Where(x => x.MessageType == ResultMessageType.Error);
    }
    
    /// <summary>
    /// Gets only the warning messages
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static IEnumerable<ResultMessage> WarningMessages(this List<ResultMessage> messages)
    {
        return messages.Where(x => x.MessageType == ResultMessageType.Warning);
    }
    
    /// <summary>
    /// Gets only the warning messages
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static IEnumerable<ResultMessage> SuccessMessages(this List<ResultMessage> messages)
    {
        return messages.Where(x => x.MessageType == ResultMessageType.Success);
    }
    
    /// <summary>
    /// Adds a handler result message
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="handlerResult"></param>
    /// <param name="message"></param>
    /// <param name="handlerResultMessageType"></param>
    public static void AddMessage<T>(this HandlerResult<T> handlerResult, string message, ResultMessageType handlerResultMessageType)
    {
        handlerResult.Messages.Add(new ResultMessage
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
    public static void AddMessage<T>(this HandlerResult<T> handlerResult, List<string> messages, ResultMessageType handlerResultMessageType)
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
    public static void AddMessage<T>(this HandlerResult<T> handlerResult, IEnumerable<string> messages, ResultMessageType handlerResultMessageType)
    {
        handlerResult.AddMessage(messages.ToList(), handlerResultMessageType);
    }

    /// <summary>
    /// Adds a handler result message
    /// </summary>
    /// <param name="authenticationResult"></param>
    /// <param name="message"></param>
    /// <param name="handlerResultMessageType"></param>
    public static void AddMessage(this AuthenticationResult authenticationResult, string message, ResultMessageType handlerResultMessageType)
    {
        authenticationResult.Messages.Add(new ResultMessage
        {
            Message = message,
            MessageType = handlerResultMessageType
        });
    }

    /// <summary>
    /// Adds many messages
    /// </summary>
    /// <param name="authenticationResult"></param>
    /// <param name="messages"></param>
    /// <param name="handlerResultMessageType"></param>
    public static void AddMessage(this AuthenticationResult authenticationResult, List<string> messages, ResultMessageType handlerResultMessageType)
    {
        foreach (var m in messages)
        {
            authenticationResult.AddMessage(m, handlerResultMessageType);
        }
    }

    /// <summary>
    /// Adds many messages
    /// </summary>
    /// <param name="authenticationResult"></param>
    /// <param name="messages"></param>
    /// <param name="handlerResultMessageType"></param>
    public static void AddMessage(this AuthenticationResult authenticationResult, IEnumerable<string> messages, ResultMessageType handlerResultMessageType)
    {
        authenticationResult.AddMessage(messages.ToList(), handlerResultMessageType);
    }
}