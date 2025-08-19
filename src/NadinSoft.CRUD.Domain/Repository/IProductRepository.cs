using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Domain.Repository;

/// <summary>
/// Defines the contract for a repository that provides data access operations for <see cref="Product"/> entities.
/// </summary>
public interface IProductRepository : IBaseRepository<Product>
{
}