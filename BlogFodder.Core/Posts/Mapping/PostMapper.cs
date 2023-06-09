using AutoMapper;
using BlogFodder.Core.Posts.Models;

namespace BlogFodder.Core.Posts.Mapping;

public class PostMapper : Profile
{
    public PostMapper()
    {
        CreateMap<Post, Post>()
            .ForMember(x => x.Categories, opt => opt.Ignore())
            .ForMember(x => x.ContentItems, opt => opt.Ignore());
    }
}