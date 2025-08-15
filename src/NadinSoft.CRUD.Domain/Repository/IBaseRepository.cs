// <copyright file="IBaseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Domain.Repository;

using System.Linq.Expressions;
using NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Defines the contract for a generic repository that provides basic data access operations.
/// </summary>
/// <typeparam name="T">
/// The type of entity handled by this repository. Must inherit from <see cref="BaseEntity"/>.
/// </typeparam>
public interface IBaseRepository<T>
    where T : BaseEntity
{
    /// <summary>
    /// Asynchronously adds a new entity to the data store.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the added entity.
    /// </returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Asynchronously retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the entity if found; otherwise, <c>null</c>.
    /// </returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Asynchronously retrieves all entities matching the specified filter.
    /// </summary>
    /// <param name="filter">
    /// An expression used to filter the entities.
    /// Pass a lambda expression like <c>x =&gt; x.IsAvailable</c>.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains a collection of matching entities.
    /// </returns>
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);

    /// <summary>
    /// Updates an existing entity in the data store.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Removes an entity from the data store.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    void Remove(T entity);

    /// <summary>
    /// Asynchronously counts the total number of entities.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result contains the total entity count.
    /// </returns>
    Task<int> CountAsync();

    /// <summary>
    /// Asynchronously determines whether any entities match the specified filter.
    /// </summary>
    /// <param name="filter">
    /// An expression used to filter the entities.
    /// Pass a lambda expression like <c>x =&gt; x.IsAvailable</c>.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation.
    /// The task result is <c>true</c> if any matching entities exist; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
}