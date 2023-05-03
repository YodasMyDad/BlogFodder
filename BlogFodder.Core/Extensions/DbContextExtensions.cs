using BlogFodder.Core.Data;
using BlogFodder.Core.Shared.Models;
using Serilog;

namespace BlogFodder.Core.Extensions;

public static class DbContextExtensions
{
    public static async Task<HandlerResult<T>> CreateOrUpdate<T>(this BlogFodderDbContext context, T entity, bool isNew, CancellationToken cancellationToken)
    {
        var crudResult = new HandlerResult<T>();
        return await CreateOrUpdate(context, entity, crudResult, isNew, cancellationToken);
    }

    public static async Task<HandlerResult<T>> CreateOrUpdate<T>(this BlogFodderDbContext context, T entity, HandlerResult<T> crudResult, bool isNew, CancellationToken cancellationToken)
    {
        if (isNew)
        {
            await context.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        }
        else
        {
            context.Update(entity);
        }

        try
        {
            var isSaved = await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            crudResult.Success = true;

            if (isSaved <= 0)
            {
                Log.Warning($"{nameof(T)} returned 0 items saved when creating or updating");
            }
        }
        catch (Exception ex)
        {
            crudResult.Success = false;
            crudResult.AddMessage(ex.Message, HandlerResultMessageType.Error);
            Log.Error(ex, $"{nameof(T)} not saved using SaveChangesAsync");
            return crudResult;
        }

        crudResult.Entity = entity;

        return crudResult;
    }
}