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
        if (entity != null)
        {
            if (isNew)
            {
                await context.AddAsync(entity, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                context.Update(entity);
            }
        
            crudResult = await context.SaveChangesAndLog(crudResult, cancellationToken);

            crudResult.Entity = entity;
        }
        else
        {
            crudResult.Success = false;
            crudResult.AddMessage("Entity to update was null", HandlerResultMessageType.Error);
        }

        return crudResult;
    }

    public static async Task<HandlerResult<T>> SaveChangesAndLog<T>(this BlogFodderDbContext context,
        HandlerResult<T> crudResult, CancellationToken cancellationToken)
    {
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
            crudResult.AddMessage($"{ex.Message} - {ex.InnerException?.Message}", HandlerResultMessageType.Error);
            Log.Error(ex, $"{nameof(T)} not saved using SaveChangesAsync");
        }

        return crudResult;
    }

    /// <summary>
    /// Returns paginated list from a queryable
    /// </summary>
    /// <param name="items"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> items, int pageIndex, int pageSize)
    {
        return new PaginatedList<T>(items, pageIndex, pageSize);
    }
}