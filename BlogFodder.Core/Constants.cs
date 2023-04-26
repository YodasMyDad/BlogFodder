namespace BlogFodder.Core;

public static class Constants
{
    public const string SettingsConfigName = "BlogFodder";
    
    public static class Claims
    {
        public const string ProfileImage = "ProfileImage";
    }
    
    public static class BackOffice
    {
        public const string NavigationSectionCore = "Core";
        public const string NavigationSectionPlugins = "Plugins";
    }
    
    public static class Files
    {
        public static readonly List<string> VideoFileTypes = new()
        {
            ".mp4",
            ".wmv",
            ".avi",
            ".mkv",
            ".mov",
            ".mpg",
            ".mpeg",
            ".m4p",
            ".m4v",
            ".avchd"
        };

        public static readonly List<string> AudioFileTypes = new()
        {
            ".mp3",
            ".m4a",
            ".wav",
            ".wma",
            ".aac"
        };

        public static readonly List<string> ImageFileTypes = new()
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".bmp",
            ".svg"
        };
    }
    
    public static class Roles
    {
        public const string AdminRoleName = "Admin";
        public const string StandardRoleName = "Standard";
        //public const string GuestRoleName = "Guest";
    }
}