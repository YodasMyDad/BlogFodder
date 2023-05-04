namespace BlogFodder.Core.Backoffice.Models;

public class Link
{
    public string? Text { get; set; }
    public string? Route { get; set; }
    public int SortOrder { get; set; }
    
    public string? Icon { get; set; }
    public bool Open { get; set; } 
    
    public string Section { get; set; } = Constants.BackOffice.NavigationSectionPlugins;

    public List<Link> SubLinks { get; set; } = new();
}