namespace BlogFodder.Core.Plugins.Models;

/// <summary>
/// Areas where plugins can be displayed
/// </summary>
public enum PluginDisplayArea
{
    HomeBeforeContent,
    HomeAfterContent,
    PostBeforeContent,
    PostAfterContent,
    HeaderTop,
    HeaderBottom,
    FooterTop,
    FooterBottom
}