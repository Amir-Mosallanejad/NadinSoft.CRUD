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
    /// Retrieves a repository instance for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for which to get the repository.
    /// Must inherit from <see cref="BaseEntity"/>.</typeparam>
    /// <returns>An instance of <see cref="IBaseRepository{TEntity}"/> for the given entity type.</returns>
    IBaseRepository<TEntity> GetRepository<TEntity>()
        where TEntity : BaseEntity;

    /// <summary>
    /// Persists all pending changes to the underlying database asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SaveChangesAsync();
}