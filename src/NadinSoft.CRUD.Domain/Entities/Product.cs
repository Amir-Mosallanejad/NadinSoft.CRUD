namespace NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents a product in the system.
/// Inherits from <see cref="BaseEntity"/>.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>
    /// A string containing the product's name.
    /// </value>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date the product was manufactured.
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> representing the production date.
    /// </value>
    public DateTime ProduceDate { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the manufacturer.
    /// </summary>
    /// <value>
    /// A string containing the manufacturer’s phone number.
    /// </value>
    public string ManufacturePhone { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email address of the manufacturer.
    /// </summary>
    /// <value>
    /// A string containing the manufacturer’s email address.
    /// </value>
    public string ManufactureEmail { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the product is available.
    /// </summary>
    /// <value>
    /// <c>true</c> if the product is available; otherwise, <c>false</c>.
    /// </value>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the product is valid.
    /// </summary>
    /// <value>
    /// <c>true</c> if the product is valid; otherwise, <c>false</c>.
    /// </value>
    public bool IsValid { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the product.
    /// </summary>
    /// <value>
    /// A guid containing the creator's user ID.
    /// </value>
    public Guid CreatedByUserId { get; set; }

    /// <summary>
    /// Gets or sets the user who created the product.
    /// </summary>
    /// <value>
    /// An instance of <see cref="ApplicationUser"/> representing the creator.
    /// </value>
    public ApplicationUser CreatedByUser { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of product validation history records
    /// associated with the product.
    /// </summary>
    public ICollection<ProductValidationHistory> ValidationHistories { get; set; } =
        new List<ProductValidationHistory>();
}