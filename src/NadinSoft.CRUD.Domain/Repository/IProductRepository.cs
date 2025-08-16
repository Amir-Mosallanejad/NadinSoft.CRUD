using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Domain.Repository;

/// <summary>
/// Defines the contract for a repository that provides data access operations for <see cref="Product"/> entities.
/// </summary>
public interface IProductRepository : IBaseRepository<Product>
{
    /// <summary>
    /// Asynchronously retrieves a paginated list of products filtered by name.
    /// </summary>
    /// <param name="name">
    /// The name (or partial name) to filter products by.
    /// If empty or <c>null</c>, all products are included.
    /// </param>
    /// <param name="page">
    /// The page number to retrieve. Must be greater than or equal to 1.
    /// </param>
    /// <param name="perpage">
    /// The number of items to include per page. Must be greater than 0.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a tuple where:
    /// <list type="bullet">
    /// <item>
    /// <description><c>Total</c>: The total number of matching products.</description>
    /// </item>
    /// <item>
    /// <description><c>Items</c>: The collection of products for the requested page.</description>
    /// </item>
    /// </list>
    /// </returns>
    Task<(int Total, IEnumerable<Product> Items)> GetProductsByFilters(
        string name,
        int page,
        int perpage);
}