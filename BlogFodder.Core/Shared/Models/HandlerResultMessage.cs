namespace BlogFodder.Core.Shared.Models;

public abstract class HandlerResultMessage
{
    public string? Message { get; set; }
    public HandlerResultMessageType MessageType { get; set; }
}