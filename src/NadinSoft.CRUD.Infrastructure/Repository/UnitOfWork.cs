using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;

namespace NadinSoft.CRUD.Infrastructure.Repository;

/// <summary>
/// Implements the <see cref="IUnitOfWork"/> interface to manage repositories and commit changes
/// to the database within a single unit of work.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// The <see cref="ApplicationDbContext"/> instance used by the repositories
    /// to interact with the database.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The <see cref="ApplicationDbContext"/> used by repositories.</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        ProductRepository = new ProductRepository(_context);
    }

    /// <summary>
    /// Gets the repository for managing <see cref="Product"/> entities.
    /// </summary>
    /// <value>The product repository.</value>
    public IBaseRepository<Product> ProductRepository { get; }

    /// <summary>
    /// Persists all changes made through the repositories to the database asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disposes the underlying database context.
    /// </summary>
    /// <remarks>
    /// This method releases all resources used by the <see cref="ApplicationDbContext"/>.
    /// After calling this method, the unit of work should not be used.
    /// </remarks>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}