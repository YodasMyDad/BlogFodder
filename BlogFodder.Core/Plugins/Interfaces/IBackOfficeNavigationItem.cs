using BlogFodder.Core.Backoffice.Models;

namespace BlogFodder.Core.Plugins.Interfaces;

public interface IBackOfficeNavigationItem
{
    public Link Link { get; set; }
}