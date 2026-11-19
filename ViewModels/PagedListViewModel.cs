namespace DemoMVC.ViewModels;

public sealed class PagedListViewModel<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public required PaginationViewModel Pagination { get; init; }
}

public sealed class PaginationViewModel
{
    public string SearchString { get; init; } = string.Empty;
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    public int FirstVisiblePage => Math.Max(1, PageNumber - 2);
    public int LastVisiblePage => Math.Min(TotalPages, PageNumber + 2);
}
