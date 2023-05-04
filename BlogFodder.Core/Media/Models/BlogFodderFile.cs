using BlogFodder.Core.Extensions;

namespace BlogFodder.Core.Media.Models;

public class BlogFodderFile
{
    /// <summary>
    /// File Id
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid().NewSequentialGuid();

    /// <summary>
    /// Url to the file, either local or cloud based
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// Type of file this is
    /// </summary>
    public BlogFodderFileType FileType { get; set; }

    /// <summary>
    /// The entity unique id that this file is related to
    /// </summary>
    public string ItemId { get; set; } = "misc";

    /// <summary>
    /// The size of the file in bytes
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Whether this is a temporary file which should be deleted
    /// </summary>
    public bool IsTemp { get; set; } = true;

    /// <summary>
    /// Date the file is created.
    /// </summary>
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Extended data saved on this file
    /// </summary>
    public Dictionary<string, object> ExtendedData { get; set; } = new();
}