using System.Net.Http.Headers;

namespace BlogFodder.Core.Settings;

public class BlogFodderSettings
{
    public string? NewUserStartingRole { get; set; }
    public string? UploadFolderName { get; set; }
    public long MaxUploadFileSizeInBytes { get; set; }
    public int MaxImageSizeInPixels { get; set; }
    public string? Favicon { get; set; }
    public List<string> AllowedFileTypes { get; set; } = new();
    public BackOfficeSettings BackOffice { get; set; } = new();
    public EmailSettings Email { get; set; } = new();
    public PluginSettings Plugins { get; set; } = new();
    public FrontEndSettings FrontEnd { get; set; } = new();
}

public class SiteNavigation
{
    public List<SiteNavigationItem> TopNav { get; set; } = new();
    public List<SiteNavigationItem> FooterNav { get; set; } = new();
}

public class SiteNavigationItem
{
    public string? Name { get; set; }
    public string? Link { get; set; }
    public bool ForceReload { get; set; }
}

public class FrontEndSettings
{
    public string? DefaultPageTitle { get; set; }
    public string? DefaultMetaDescription { get; set; }
    public SiteNavigation SiteNavigation { get; set; } = new();
    public string? HeaderComponent { get; set; }
    public string? FooterComponent { get; set; }
    public string? PostListComponent { get; set; }
    public string? PostListItemComponent { get; set; }
    public string? PostViewComponent { get; set; }
    public List<string> Styles { get; set; } = new();
    public List<string> Scripts { get; set; } = new();
}

public class PluginSettings
{
    // ReSharper disable once InconsistentNaming
    public string? IStorageProvider { get; set; }
}

public class EmailSettings
{
    public string? SenderEmail { get; set; }
    public SmtpSettings Smtp { get; set; } = new();
}

public class SmtpSettings
{
    public string? Host { get; set; }
    public int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class BackOfficeSettings
{
    public List<string> NavigationSections { get; set; } = new();
}