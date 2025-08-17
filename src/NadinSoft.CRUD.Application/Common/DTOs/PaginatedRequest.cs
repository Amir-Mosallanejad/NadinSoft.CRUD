namespace NadinSoft.CRUD.Application.Common.DTOs;

/// <summary>
/// Represents the base class for requests that support pagination.
/// </summary>
public abstract class PaginatedRequest
{
    /// <summary>
    /// Gets or sets the page number to retrieve.
    /// </summary>
    /// <value>
    /// The page number, starting from 1. Defaults to 1.
    /// </value>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <value>
    /// The number of items to include in each page. Defaults to 10.
    /// </value>
    public int PerPage { get; set; } = 10;
}