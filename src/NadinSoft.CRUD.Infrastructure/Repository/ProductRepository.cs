using Microsoft.EntityFrameworkCore;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;
using NadinSoft.CRUD.Infrastructure.Data;

namespace NadinSoft.CRUD.Infrastructure.Repository;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<(int Total, IEnumerable<Product> Items)> GetProductsByFilters(string name, int page, int perPage)
    {
        IQueryable<Product> query = DbSet.AsQueryable();

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