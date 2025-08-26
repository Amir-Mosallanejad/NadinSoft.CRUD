using MediatR;

namespace NadinSoft.CRUD.Application.Events.ProductValidationChanged;

/// <summary>
/// Represents an event that is raised whenever a product's validation status changes.
/// Implements <see cref="INotification"/> for use with MediatR.
/// </summary>
public class ProductValidationChangedEvent : INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductValidationChangedEvent"/> class.
    /// </summary>
    /// <param name="productId">The ID of the product whose validation status changed.</param>
    /// <param name="userId">The ID of the user performing the change.</param>
    /// <param name="oldValue">The previous validation status, or <c>null</c> if not applicable.</param>
    /// <param name="newValue">The new validation status of the product.</param>
    public ProductValidationChangedEvent(Guid productId, Guid userId, bool? oldValue, bool newValue)
    {
        ProductId = productId;
        UserId = userId;
        OldValue = oldValue;
        NewValue = newValue;
        ModifyDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the identifier of the product whose validation status has changed.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the identifier of the user who performed the validation change.
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// Gets the previous validation status of the product.
    /// <c>null</c> if there was no previous value.
    /// </summary>
    public bool? OldValue { get; }

    /// <summary>
    /// Gets a value indicating whether gets the new validation status of the product.
    /// </summary>
    public bool NewValue { get; }

    /// <summary>
    /// Gets the timestamp when the validation change occurred.
    /// </summary>
    public DateTime ModifyDate { get; }
}