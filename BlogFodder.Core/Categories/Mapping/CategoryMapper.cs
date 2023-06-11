using AutoMapper;
using BlogFodder.Core.Categories.Models;

namespace BlogFodder.Core.Categories.Mapping;

public class CategoryMapper : Profile
{
    public CategoryMapper()
    {
        CreateMap<Category, Category>()
            .ForMember(x => x.Posts, opt => opt.Ignore())
            .ForMember(x => x.SocialImage, opt => opt.Ignore());
    }
}