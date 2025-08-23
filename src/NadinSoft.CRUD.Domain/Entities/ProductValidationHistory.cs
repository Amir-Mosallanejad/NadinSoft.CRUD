using System.ComponentModel.DataAnnotations.Schema;

namespace NadinSoft.CRUD.Domain.Entities;

/// <summary>
/// Represents a history record of product validation changes.
/// </summary>
public class ProductValidationHistory : BaseEntity
{
    /// <summary>
    /// Gets or sets the previous validation value.
    /// </summary>
    public bool? OldValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the new validation value.
    /// </summary>
    public bool NewValue { get; set; }

    /// <summary>
    /// Gets or sets the date when the change was made.
    /// </summary>
    public DateTime ModifyDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who made the change.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the user who made the change.
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the product associated with this change.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product associated with this change.
    /// </summary>
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
}