using Microsoft.AspNetCore.Identity;

namespace NadinSoft.CRUD.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Product> Products { get; set; } = new List<Product>();
}