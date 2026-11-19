using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Models;

public static class PaginationOptions
{
    public const int DefaultPageSize = 10;

    public static IReadOnlyList<int> PageSizes { get; } =
        Array.AsReadOnly(new[] { 5, 10, 20, 50 });
}

public static class SearchPattern
{
    public const string EscapeCharacter = "\\";

    public static string CreateContainsPattern(string value)
    {
        var escapedValue = value
            .Replace(EscapeCharacter, EscapeCharacter + EscapeCharacter)
            .Replace("%", EscapeCharacter + "%")
            .Replace("_", EscapeCharacter + "_")
            .Replace("[", EscapeCharacter + "[");

        return $"%{escapedValue}%";
    }
}

public sealed class PaginatedList<T>
{
    private PaginatedList(
        IReadOnlyList<T> items,
        int pageNumber,
        int pageSize,
        int totalCount,
        int totalPages)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = totalPages;
    }

    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var normalizedPageSize = PaginationOptions.PageSizes.Contains(pageSize)
            ? pageSize
            : PaginationOptions.DefaultPageSize;
        var totalCount = await query.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)normalizedPageSize);
        var normalizedPageNumber = Math.Clamp(pageNumber, 1, Math.Max(totalPages, 1));
        var items = await query
            .Skip((normalizedPageNumber - 1) * normalizedPageSize)
            .Take(normalizedPageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(
            items,
            normalizedPageNumber,
            normalizedPageSize,
            totalCount,
            totalPages);
    }
}
