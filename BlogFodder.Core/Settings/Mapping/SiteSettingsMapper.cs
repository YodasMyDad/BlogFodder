using AutoMapper;
using BlogFodder.Core.Settings.Models;

namespace BlogFodder.Core.Settings.Mapping;

public class SiteSettingsMapper : Profile
{
    public SiteSettingsMapper()
    {
        CreateMap<SiteSettings, SiteSettings>()
            .ForMember(x => x.Logo, opt => opt.Ignore());
    }
}