using AutoMapper;
using BlogFodder.Core.Membership.Models;

namespace BlogFodder.Core.Membership.Mapping;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, User>()
            .ForMember(x => x.ProfileImage, opt => opt.Ignore())
            .ForMember(x => x.Posts, opt => opt.Ignore())
            .ForMember(x => x.UserRoles, opt => opt.Ignore());
    }
}