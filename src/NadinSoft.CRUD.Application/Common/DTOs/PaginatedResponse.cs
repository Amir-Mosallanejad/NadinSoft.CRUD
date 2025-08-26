namespace NadinSoft.CRUD.Application.Common.DTOs;

/// <summary>
/// Represents a paginated response containing a collection of items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of items in the response. Must be a reference type.</typeparam>
public class PaginatedResponse<T>
    where T : class
{
    /// <summary>
    /// Gets or sets the collection of items in the current page.
    /// </summary>
    /// <value>A list of items of type <typeparamref name="T"/>.</value>
    public List<T> Items { get; set; } = null!;

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    /// <value>The current page number, starting from 1.</value>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <value>The number of items included in each page.</value>
    public int PerPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    /// <value>The total count of items available in the data source.</value>
    public int TotalCount { get; set; }
}