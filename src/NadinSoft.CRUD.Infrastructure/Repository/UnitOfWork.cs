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
    private readonly ApplicationDbContext _context;

    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IBaseRepository<TEntity> GetRepository<TEntity>()
        where TEntity : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return (IBaseRepository<TEntity>)_repositories[typeof(TEntity)];
        }

        IBaseRepository<TEntity> repository = new BaseRepository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repository);

        return repository;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}