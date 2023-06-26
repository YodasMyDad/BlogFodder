namespace BlogFodder.Core.Plugins.Interfaces;

public interface IAdminDashboard
{
    /// <summary>
    /// Optional heading for the dashboard
    /// </summary>
    string? Heading { get; }

    /// <summary>
    /// Optional description for the dashboard
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// Icon which is displayed in the top left of the dashboard
    /// </summary>
    string Icon { get; }
    
    /// <summary>
    /// The amount of columns you want the dashboard to take up
    /// </summary>
    int Columns { get; }
    
    /// <summary>
    /// The sort order of the dashboard
    /// </summary>
    int SortOrder { get; }
}