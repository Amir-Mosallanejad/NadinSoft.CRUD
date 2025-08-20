using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Domain.Repository;

/// <summary>
/// Represents a unit of work that encapsulates repositories and ensures
/// atomic operations on the database. Provides a mechanism to commit changes
/// and dispose of resources.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IBaseRepository<TEntity> GetRepository<TEntity>()
        where TEntity : BaseEntity;

    Task SaveChangesAsync();
}