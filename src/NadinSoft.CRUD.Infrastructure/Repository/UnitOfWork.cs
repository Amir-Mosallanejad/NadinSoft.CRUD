using Microsoft.EntityFrameworkCore.Storage;
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
    /// The application's database context used to interact with the data store.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Stores already created repositories by entity type to ensure reusability.
    /// </summary>
    private readonly Dictionary<Type, object> _repositories = new();

    /// <summary>
    /// Holds the current database transaction if one has been started.
    /// </summary>
    private IDbContextTransaction? _currentTransaction;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context for data access.</param>
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public IBaseRepository<TEntity> GetRepository<TEntity>()
        where TEntity : BaseEntity
    {
        if (_repositories.TryGetValue(typeof(TEntity), out var repository))
        {
            return (IBaseRepository<TEntity>)repository;
        }

        IBaseRepository<TEntity> newRepository = new BaseRepository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), newRepository);

        return newRepository;
    }

    /// <inheritdoc />
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    /// <inheritdoc />
    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            return;
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    /// <inheritdoc />
    public void BeginTransaction()
    {
        if (_currentTransaction != null)
        {
            return;
        }

        _currentTransaction = _context.Database.BeginTransaction();
    }

    /// <inheritdoc />
    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_currentTransaction != null)
            {
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    /// <inheritdoc />
    public void Commit()
    {
        try
        {
            _context.SaveChanges();
            if (_currentTransaction != null)
            {
                _currentTransaction.Commit();
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }
        catch
        {
            Rollback();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task RollbackAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    /// <inheritdoc />
    public void Rollback()
    {
        if (_currentTransaction != null)
        {
            _currentTransaction.Rollback();
            _currentTransaction.Dispose();
            _currentTransaction = null;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}