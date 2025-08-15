// <copyright file="BaseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Infrastructure.Repository;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;

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
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
        this.DbSet = this.Context.Set<T>();
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
        await this.DbSet.AddAsync(entity);
        await this.Context.SaveChangesAsync();
        return entity;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        return await this.DbSet.AnyAsync(filter);
    }

    /// <inheritdoc/>
    public virtual async Task<int> CountAsync()
    {
        return await this.DbSet.CountAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await this.DbSet.Where(filter).ToListAsync();
    }

    /// <inheritdoc/>
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await this.DbSet.FindAsync(id);
    }

    /// <inheritdoc/>
    public virtual void Remove(T entity)
    {
        this.DbSet.Remove(entity);
        this.Context.SaveChanges();
    }

    /// <inheritdoc/>
    public virtual void Update(T entity)
    {
        this.DbSet.Update(entity);
        this.Context.SaveChanges();
    }
}