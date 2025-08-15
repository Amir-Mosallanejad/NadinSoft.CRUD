// <copyright file="ProductRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Infrastructure.Repository;

using Microsoft.EntityFrameworkCore;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;

/// <summary>
/// Repository implementation for <see cref="Product"/> entities,
/// extending <see cref="BaseRepository{T}"/> and implementing <see cref="IProductRepository"/>.
/// </summary>
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> used for database access.</param>
    public ProductRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    /// <summary>
    /// Retrieves products filtered by name with pagination.
    /// </summary>
    /// <param name="name">The optional name filter. Case-insensitive partial matches are applied.</param>
    /// <param name="page">The page number (1-based) for pagination.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <returns>
    /// A tuple containing the total number of filtered products and the list of products for the requested page.
    /// </returns>
    public async Task<(int Total, IEnumerable<Product> Items)> GetProductsByFilters(string name, int page, int perPage)
    {
        IQueryable<Product> query = this.DbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(x => x.Name.ToLower().Contains(name));
        }

        int totalCount = await query.CountAsync();

        List<Product> result = await query
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync();

        return (totalCount, result);
    }
}