using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Core.Extensions;

public static class EditorExtensions
{
    /// <summary>
    /// Returns a list of CSS classes for the default IEditorSettings
    /// </summary>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static string PaddingMargin(this IEditorSettings settings)
    {
        var items = new List<string>();
        if (settings.PaddingTop > 0)
        {
            items.Add($"pt-{settings.PaddingTop}");
        }
        if (settings.PaddingBottom > 0)
        {
            items.Add($"pb-{settings.PaddingBottom}");
        }
        if (settings.MarginTop > 0)
        {
            items.Add($"mt-{settings.MarginTop}");
        }
        if (settings.MarginBottom > 0)
        {
            items.Add($"mb-{settings.MarginBottom}");
        }

        return string.Join(" ", items.Select(x => x));
    }
}