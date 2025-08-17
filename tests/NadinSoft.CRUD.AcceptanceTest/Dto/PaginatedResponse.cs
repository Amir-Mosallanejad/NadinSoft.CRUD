namespace NadinSoft.CRUD.AcceptanceTest.Dto;

/// <summary>
/// Represents a paginated response containing a subset of items and pagination information.
/// </summary>
/// <typeparam name="T">The type of items contained in the response.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// Gets or sets the items on the current page.
    /// </summary>
    /// <value>
    /// A list of items of type <typeparamref name="T"/> contained in the current page.
    /// </value>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    /// <value>
    /// An integer representing the total number of items.
    /// </value>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number (1-based index).
    /// </summary>
    /// <value>
    /// An integer representing the current page number.
    /// </value>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <value>
    /// An integer representing the page size.
    /// </value>
    public int PageSize { get; set; }
}