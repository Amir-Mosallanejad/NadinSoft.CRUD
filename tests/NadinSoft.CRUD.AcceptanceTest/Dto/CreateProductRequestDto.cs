namespace NadinSoft.CRUD.AcceptanceTest.Dto;

/// <summary>
/// Data Transfer Object (DTO) representing a request to create a new product.
/// </summary>
public class CreateProductRequestDto
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>
    /// A non-empty product name.
    /// </value>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date the product was produced.
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> value not in the future.
    /// </value>
    public DateTime ProduceDate { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the manufacturer.
    /// </summary>
    /// <value>
    /// The manufacturer's phone number (<c>+98912...</c>).
    /// </value>
    public string ManufacturePhone { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email address of the manufacturer.
    /// </summary>
    /// <value>
    /// A valid email address for the manufacturer.
    /// </value>
    public string ManufactureEmail { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the product is available.
    /// </summary>
    /// <value>
    /// <c>true</c> if the product is available; otherwise, <c>false</c>.
    /// </value>
    public bool IsAvailable { get; set; }
}