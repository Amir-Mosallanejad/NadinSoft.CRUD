using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;

namespace NadinSoft.CRUD.Infrastructure.Repository;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> DbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = Context.Set<T>();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        return await DbSet.AnyAsync(filter);
    }

    public virtual async Task<int> CountAsync()
    {
        return await DbSet.CountAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
    {
        return await DbSet.Where(filter).ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual void Remove(T entity)
    {
        DbSet.Remove(entity);
        Context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        DbSet.Update(entity);
        Context.SaveChanges();
    }
}