namespace NadinSoft.CRUD.AcceptanceTest.Dto;

/// <summary>
/// Represents the data transfer object used to update an existing product.
/// </summary>
public class UpdateProductRequestDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    /// <value>
    /// A <see cref="Guid"/> representing the product's ID.
    /// </value>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> containing the product's name.
    /// </value>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date when the product was produced.
    /// </summary>
    /// <value>
    /// A <see cref="DateTime"/> representing the produce date.
    /// </value>
    public DateTime ProduceDate { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the manufacturer.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> containing the manufacturer's phone number.
    /// </value>
    public string ManufacturePhone { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email address of the manufacturer.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> containing the manufacturer's email.
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