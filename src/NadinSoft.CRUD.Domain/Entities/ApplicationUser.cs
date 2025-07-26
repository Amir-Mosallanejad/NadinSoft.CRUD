using Microsoft.AspNetCore.Identity;

namespace NadinSoft.CRUD.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public override string? UserName { get; set; }
    public override string? Email { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}