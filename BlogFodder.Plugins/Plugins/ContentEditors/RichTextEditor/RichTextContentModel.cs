﻿using BlogFodder.Core.Plugins.Interfaces;

namespace BlogFodder.Plugins.Plugins.ContentEditors.RichTextEditor;

public class RichTextContentModel : IPluginSettings
{
    public string Testing { get; set; } = "Initial Data";
}