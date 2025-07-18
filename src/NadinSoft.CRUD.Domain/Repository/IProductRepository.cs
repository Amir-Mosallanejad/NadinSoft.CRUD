using NadinSoft.CRUD.Domain.Entities;

namespace NadinSoft.CRUD.Domain.Repository;

public interface IProductRepository : IBaseRepository<Product>
{
    Task<(int Total, IEnumerable<Product> Items)> GetProductsByFilters(string name, int page,
        int perpage);
}