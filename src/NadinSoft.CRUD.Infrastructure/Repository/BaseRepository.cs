using Microsoft.EntityFrameworkCore;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;
using System.Linq.Expressions;

namespace NadinSoft.CRUD.Infrastructure.Repository;

/// <summary>
/// Provides a base repository implementation for CRUD operations on entities of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the entity, which must inherit from <see cref="BaseEntity"/>.</typeparam>
public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> to use for database access.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="context"/> is null.</exception>
    protected BaseRepository(ApplicationDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = Context.Set<T>();
    }

    /// <summary>
    /// Gets the <see cref="ApplicationDbContext"/> instance used for database operations.
    /// </summary>
    protected ApplicationDbContext Context { get; }

    /// <summary>
    /// Gets the <see cref="DbSet{TEntity}"/> representing the entity set.
    /// </summary>
    protected DbSet<T> DbSet { get; }

    /// <inheritdoc/>
    public virtual async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        return await DbSet.AnyAsync(filter);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync()
    {
        return await DbSet.CountAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await DbSet.Where(filter).ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    /// <inheritdoc/>
    public virtual void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    /// <inheritdoc/>
    public virtual void Update(T entity)
    {
        DbSet.Update(entity);
    }

    /// <inheritdoc />
    public virtual async Task<(int Total, IEnumerable<T> Items)> GetByFiltersAsync(
        Expression<Func<T, bool>>? filter = null,
        int page = 1,
        int perPage = 10)
    {
        IQueryable<T> query = Context.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        int totalCount = await query.CountAsync();

        List<T> result = await query
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync();

        return (totalCount, result);
    }
}