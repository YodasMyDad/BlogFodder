using AutoMapper;
using BlogFodder.Core.Categories.Commands;
using BlogFodder.Core.Categories.Models;
using BlogFodder.Core.Data;
using BlogFodder.Core.Extensions;
using BlogFodder.Core.Providers;
using BlogFodder.Core.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogFodder.Core.Categories.Handlers;

public class CreateUpdateCategoryHandler : IRequestHandler<CreateUpdateCategoryCommand, HandlerResult<Category>>
{
    private readonly SlugHelper _slugHelper;
    private readonly ProviderService _providerService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    
    public CreateUpdateCategoryHandler(ProviderService providerService, IServiceProvider serviceProvider, IMapper mapper)
    {
        _providerService = providerService;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _slugHelper = new SlugHelper();
    }

    public async Task<HandlerResult<Category>> Handle(CreateUpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var handlerResult = new HandlerResult<Category>();

        // Set any empty values like the Url
        if (request.Category.Url.IsNullOrWhiteSpace())
        {
            request.Category.Url = _slugHelper.GenerateSlug(request.Category.Name);
        }

        if (request.Category.PageTitle.IsNullOrWhiteSpace())
        {
            request.Category.PageTitle = request.Category.Name;
        }

        if (!request.IsUpdate)
        {
            request.Category.DateCreated = DateTime.UtcNow;
            request.Category.DateUpdated = DateTime.UtcNow;
        }
        else
        {
            request.Category.DateUpdated = DateTime.UtcNow;
        }
        
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BlogFodderDbContext>();
        
        var category = dbContext.Categories
            .Include(x => x.SocialImage)
            .FirstOrDefault(x => x.Id == request.Category.Id);
        
        var oldSocialImageId = category?.SocialImageId;
        
        category ??= new Category();

        _mapper.Map(request.Category, category);
        
        if (!request.IsUpdate)
        {
            dbContext.Categories.Add(category);
        }
        
        if (request.SocialImage != null)
        {
            var socialImageFile =
                await request.SocialImage.AddFileToDb(request.Category.Id, handlerResult, _providerService, dbContext);
            category.SocialImage = socialImageFile;
            category.SocialImageId = socialImageFile?.Id;

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
        
        return await dbContext.SaveChangesAndLog(category, handlerResult, cancellationToken);
    }
    
}