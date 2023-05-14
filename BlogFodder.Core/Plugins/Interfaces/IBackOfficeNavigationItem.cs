using BlogFodder.Core.Backoffice.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

/// <summary>
/// Allows you to add a navigation item to a specific section in the backoffice
/// </summary>
public interface IBackOfficeNavigationItem
{
    public Link Link { get; set; }
}