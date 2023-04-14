namespace BlogFodder.Core.Backoffice.Models;

public class NavigationSection
{
    public string? Name { get; set; }
    public List<Link> Links { get; set; } = new();
}