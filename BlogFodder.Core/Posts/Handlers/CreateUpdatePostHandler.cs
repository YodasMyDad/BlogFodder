using AutoMapper;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Media.Models;
using BlogFodder.Core.Posts.Commands;
using BlogFodder.Core.Posts.Models;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Posts.Handlers;

public class CreateUpdatePostHandler : IRequestHandler<CreateUpdatePostCommand, HandlerResult<Post>>
{
    private readonly SlugHelper _slugHelper;
    private readonly ProviderService _providerService;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateUpdatePostHandler(ProviderService providerService, 
        IServiceProvider serviceProvider, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _providerService = providerService;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _slugHelper = new SlugHelper();
    }

    public async Task<HandlerResult<Post>> Handle(CreateUpdatePostCommand request, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<BlogFodderDbContext>();

        var handlerResult = new HandlerResult<Post>();

        // Set any empty values like the Url
        if (request.Post.Url.IsNullOrWhiteSpace())
        {
            request.Post.Url = _slugHelper.GenerateSlug(request.Post.Name);
        }

        if (request.Post.MetaDescription.IsNullOrWhiteSpace())
        {
            request.Post.MetaDescription = request.Post.Excerpt;
        }

        if (request.Post.PageTitle.IsNullOrWhiteSpace())
        {
            request.Post.PageTitle = request.Post.Name;
        }

        if (!request.IsUpdate)
        {
            request.Post.DateCreated = DateTime.UtcNow;
            request.Post.DateUpdated = DateTime.UtcNow;
        }
        else
        {
            request.Post.DateUpdated = DateTime.UtcNow;
        }
        
        var post = dbContext!.Posts
            .Include(x => x.Categories)
            .Include(x => x.ContentItems)
            .Include(x => x.FeaturedImage)
            .Include(x => x.SocialImage)
            .FirstOrDefault(x => x.Id == request.Post.Id);
        
        var oldFeaturedImageId = post?.FeaturedImageId;
        var oldSocialImageId = post?.SocialImageId;
        
        post ??= new Post();

        _mapper.Map(request.Post, post);
        
        if (!request.IsUpdate)
        {
            // Set the user it's created by too
            var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
            if (userId != null)
            {
                var user = dbContext.Users.FirstOrDefault(x => x.Id == userId);
                post.User = user;
                post.UserId = userId;
            }

            dbContext.Posts.Add(post);
        }

        // If new images are provided, save them and update the post properties
        if (request.FeaturedImage != null)
        {
            var featuredImageFile =
                await request.FeaturedImage.AddFileToDb(request.Post.Id, handlerResult, _providerService, dbContext);
            post.FeaturedImage = featuredImageFile;
            post.FeaturedImageId = featuredImageFile?.Id;

            // If an old image existed, delete it
            if (oldFeaturedImageId != null)
            {
                var oldFeaturedImage = await dbContext.Files.FindAsync(new object?[] {oldFeaturedImageId},
                    cancellationToken: cancellationToken);
                if (oldFeaturedImage != null)
                {
                    dbContext.Files.Remove(oldFeaturedImage);
                }
            }
        }

        if (request.SocialImage != null)
        {
            var socialImageFile =
                await request.SocialImage.AddFileToDb(request.Post.Id, handlerResult, _providerService, dbContext);
            post.SocialImage = socialImageFile;
            post.SocialImageId = socialImageFile?.Id;

            // If an old image existed, delete it
            if (oldSocialImageId != null)
            {
                var oldSocialImage = await dbContext.Files.FindAsync(new object?[] {oldSocialImageId},
                    cancellationToken: cancellationToken);
                if (oldSocialImage != null)
                {
                    dbContext.Files.Remove(oldSocialImage);
                }
            }
        }

        // Sort out the categories
        var updatedCategoryIds = request.Post.Categories.Select(c => c.Id).ToList();
        var existingCategories = dbContext.Categories.AsNoTracking().Where(x => x.Posts.Any(p => p.Id == post.Id)).ToList();

        // Add new categories
        foreach (var categoryId in updatedCategoryIds)
        {
            if (existingCategories.All(c => c.Id != categoryId))
            {
                var categoryToAdd = await dbContext.Categories.FirstOrDefaultAsync(x=> x.Id == categoryId, cancellationToken: cancellationToken);
                if (categoryToAdd != null) post.Categories.Add(categoryToAdd);
            }
        }

        // Remove unselected categories
        for (var i = existingCategories.Count - 1; i >= 0; i--)
        {
            var category = existingCategories[i];
            if (!updatedCategoryIds.Contains(category.Id))
            {
                post.Categories.Remove(category);
            }
        }

        // Sort out the content items
        // Handle ContentItems
        var updatedContentItems = request.Post.ContentItems;

        // Add or update content items
        foreach (var updatedItem in updatedContentItems)
        {
            var existingItem = post.ContentItems.FirstOrDefault(ci => ci.Id == updatedItem.Id);
            if (existingItem != null)
            {
                // Existing item, update properties
                existingItem.SortOrder = updatedItem.SortOrder;
                existingItem.PluginAlias = updatedItem.PluginAlias;
                existingItem.PluginData = updatedItem.PluginData;
                existingItem.PluginSettings = updatedItem.PluginSettings;
            }
            else
            {
                // New item, add it to the collection
                post.ContentItems.Add(updatedItem);
            }
        }

        // Remove deleted content items
        for (var i = post.ContentItems.Count - 1; i >= 0; i--)
        {
            var existingItem = post.ContentItems[i];
            if (updatedContentItems.All(ci => ci.Id != existingItem.Id))
            {
                post.ContentItems.Remove(existingItem);
            }
        }
        
        return await dbContext.SaveChangesAndLog(post, handlerResult, cancellationToken)
            .ConfigureAwait(false);
    }
}