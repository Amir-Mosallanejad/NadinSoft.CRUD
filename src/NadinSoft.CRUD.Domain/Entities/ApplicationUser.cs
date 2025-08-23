using Microsoft.AspNetCore.Identity;

namespace NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents an application user with additional properties and relationships.
/// Inherits from <see cref="IdentityUser"/>.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    /// <summary>
    /// Gets or sets the username used to log in to the application.
    /// </summary>
    /// <value>
    /// A string representing the user's login name.
    /// </value>
    public override string? UserName { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    /// <value>
    /// The user's email address, or <c>null</c> if not set.
    /// </value>
    public override string? Email { get; set; }

    /// <summary>
    /// Gets or sets the collection of products associated with the user.
    /// </summary>
    /// <value>
    /// A collection of <see cref="Product"/> entities owned or created by the user.
    /// </value>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}