namespace BlogFodder.Core.Settings;

public class BlogFodderSettings
{
    public BackOfficeSettings BackOffice { get; set; } = new();
}

public class BackOfficeSettings
{
    public List<string> NavigationSections { get; set; } = new();
}