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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Posts.Handlers;

public class CreateUpdatePostHandler : IRequestHandler<CreateUpdatePostCommand, HandlerResult<Post>>
{
    private readonly SlugHelper _slugHelper;
    private readonly ProviderService _providerService;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public CreateUpdatePostHandler(ProviderService providerService, BlogFodderDbContext dbContext,
        IServiceProvider serviceProvider, IMapper mapper)
    {
        _providerService = providerService;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
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

        // See if this post already exists as we will need to remove the images
        var post = dbContext!.Posts
            .Include(x => x.Categories) // TODO - Not sure if I need to remove this part
            .Include(x => x.ContentItems)
            .Include(x => x.FeaturedImage)
            .Include(x => x.SocialImage)
            .FirstOrDefault(x => x.Id == request.Post.Id);
        
        var oldFeaturedImageId = post?.FeaturedImageId;
        var oldSocialImageId = post?.SocialImageId;

        // Map if update
        post ??= new Post();

        _mapper.Map(request.Post, post);
        
        if (!request.IsUpdate)
        {
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
        
        return await dbContext.SaveChangesAndLog(handlerResult, cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task HandleImageUpdate(
        IBrowserFile? newImage,
        Guid? oldImageId,
        Action<Post, BlogFodderFile> setImage,
        Action<Post, Guid?> setImageId,
        Post post,
        BlogFodderDbContext dbContext,
        HandlerResult<Post> handlerResult,
        CancellationToken cancellationToken)
    {
        if (newImage != null)
        {
            var imageFile = await newImage.AddFileToDb(post.Id, handlerResult, _providerService, dbContext);

            // Update the post properties
            setImage(post, imageFile);
            setImageId(post, imageFile.Id);

            // If an old image existed, delete it
            if (oldImageId != null)
            {
                var oldImage =
                    await dbContext.Files.FindAsync(new object?[] {oldImageId}, cancellationToken: cancellationToken);
                if (oldImage != null)
                {
                    dbContext.Files.Remove(oldImage);
                }
            }
        }
    }
}


/*if (_providerService.StorageProvider != null)
{
    // Profile Image - Need to save image and then create a file
    if (request.FeaturedImage != null)
    {
        // Delete the old one if there is one
        if (post?.FeaturedImage != null)
        {
            fileIdToDelete.Add(post.FeaturedImage.Id);
        }
        
        // Save the file, create a file and attach it to the user
        var fileResult = await request.FeaturedImage.AddFileToDb(request.Post.Id, handlerResult, _providerService, dbContext);

        // Set the file to the user
        request.Post.FeaturedImage = fileResult;
        
    }
    else if(post?.FeaturedImage != null 
            && request.Post.FeaturedImageId == null)
    {
        // Delete the image
        fileIdToDelete.Add(post.FeaturedImage.Id);
        post.FeaturedImageId = null;
        post.FeaturedImage = null;
    }

    if (request.SocialImage != null)
    {
        // Delete the old one if there is one
        if (post?.SocialImage != null)
        {
            fileIdToDelete.Add(post.SocialImage.Id);
        }
        
        // Save the file, create a file and attach it to the user
        var fileResult = await request.SocialImage.AddFileToDb(request.Post.Id, handlerResult, _providerService, dbContext);

        // Set the file to the user
        request.Post.SocialImage = fileResult;
        
    }
    else if(post?.SocialImage != null 
            && request.Post.SocialImageId == null)
    {
        // Delete the image
        fileIdToDelete.Add(post.SocialImage.Id);
        post.SocialImageId = null;
        post.SocialImage = null;
    }

    if (post != null && fileIdToDelete.Any())
    {
        foreach (var guid in fileIdToDelete)
        {
            var file = dbContext.Files.FirstOrDefault(x => x.Id == guid);
            if (file != null)
            {
                dbContext.Files.Remove(file);
            }
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

// Handle categories
var updatedCategories = request.Post.Categories;
var existingCategories = post.Categories;

// Add new categories
foreach (var category in updatedCategories)
{
    if (!existingCategories.Any(c => c.Id == category.Id))
    {
        post.Categories.Add(category);
    }
}

// Remove unselected categories
for (var i = existingCategories.Count - 1; i >= 0; i--)
{
    var category = existingCategories[i];
    if (updatedCategories.All(c => c.Id != category.Id))
    {
        post.Categories.Remove(category);
    }
}*/