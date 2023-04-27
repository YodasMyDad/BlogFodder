namespace BlogFodder.Core.Shared.Models;

public class PaginatedList<T>
{
    public int PageIndex { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

    public PaginatedList()
    {
    }

    public PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        Items = items;
        TotalItems = count;
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;
}