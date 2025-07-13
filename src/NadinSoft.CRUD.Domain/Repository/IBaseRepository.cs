using System.Linq.Expressions;
using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Domain.Repository;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> AddAsync(T entity);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    void Update(T entity);
    void Remove(T entity);
    Task<int> CountAsync();
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
}