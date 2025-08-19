using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Domain.Repository;

/// <summary>
/// Represents a unit of work that encapsulates repositories and ensures
/// atomic operations on the database. Provides a mechanism to commit changes
/// and dispose of resources.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository for managing <see cref="Product"/> entities.
    /// </summary>
    /// <value>The product repository.</value>
    IBaseRepository<Product> ProductRepository { get; }

    /// <summary>
    /// Persists all changes made through the repositories to the database asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SaveChangesAsync();
}