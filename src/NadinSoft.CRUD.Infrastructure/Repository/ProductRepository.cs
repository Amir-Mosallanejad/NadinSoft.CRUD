using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;

namespace NadinSoft.CRUD.Infrastructure.Repository;

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
}