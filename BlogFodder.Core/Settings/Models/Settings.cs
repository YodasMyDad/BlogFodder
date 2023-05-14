namespace BlogFodder.Core.Settings.Models;

public class Settings
{
    public static string Alias => "SiteSettings";

    public string SiteName { get; set; } = "BlogFodder";
    public string DefaultPageTitle { get; set; } = "BlogFodder - Blazor Blog Engine";
    public string DefaultMetaDescription { get; set; } = "A plugin based blog engine built in .Net C# Blazor";

    public int HomeAmountPerPage { get; set; } = 10;
}