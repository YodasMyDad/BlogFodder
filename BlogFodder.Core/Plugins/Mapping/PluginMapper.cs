using AutoMapper;
using BlogFodder.Core.Plugins.Models;

namespace BlogFodder.Core.Plugins.Mapping;

public class PluginMapper : Profile
{
    public PluginMapper()
    {
        CreateMap<Plugin, Plugin>();
    }
}