using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Settings.Models;
using BlogFodder.Core.Shared.Models;
using MediatR;

namespace BlogFodder.Core.Posts.Commands;


public class GetPostsCommand : IRequest<PaginatedList<Post>>
{
    public GetPostsCommand(SiteSettings siteSettings)
    {
        AmountPerPage = siteSettings.HomeAmountPerPage;
    }
    public GetPostsCommand(int amountPerPage)
    {
        AmountPerPage = amountPerPage;
    }
    
    public bool AsNoTracking { get; set; } = true;
    public List<Guid> CategoryIds { get; set; } = new();
    public Guid? UserId { get; set; }
    public int PageIndex { get; set; } = 1;
    public int AmountPerPage { get; set; }
    public bool IncludeCategories { get; set; } = true;
    public bool IncludeFeaturedImage { get; set; } = true;
    public bool IncludeSocialImage { get; set; } = false;
    public bool IncludeContentItems { get; set; } = false;
    public bool IncludeUsers { get; set; } = true;
    public string? SearchTerm { get; set; }
    public GetPostsOrderBy OrderBy { get; set; } = GetPostsOrderBy.DateUpdatedDescending;
}

public enum GetPostsOrderBy
{
    DateUpdated,
    DateUpdatedDescending,
    DateCreated,
    DateCreatedDescending
}