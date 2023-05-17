namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Interface for IPlugin > ContentPlugin > Component
/// </summary>
public interface IPluginContentComponent
{
    string? Model { get; set; }
    string? Settings { get; set; }
    Guid? PostId { get; set; }
}