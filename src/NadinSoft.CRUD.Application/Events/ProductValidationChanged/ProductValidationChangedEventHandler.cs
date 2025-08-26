using MediatR;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Domain.Entities;
using NadinSoft.CRUD.Domain.Repository;

namespace NadinSoft.CRUD.Application.Events.ProductValidationChanged;

/// <summary>
/// Handles <see cref="ProductValidationChangedEvent"/> notifications
/// by saving the validation change history to the database.
/// Implements <see cref="INotificationHandler{TNotification}"/> for use with MediatR.
/// </summary>
public class ProductValidationChangedEventHandler(
    IUnitOfWork unitOfWork,
    ILogger<ProductValidationChangedEventHandler> logger)
    : INotificationHandler<ProductValidationChangedEvent>
{
    /// <summary>
    /// Handles the <see cref="ProductValidationChangedEvent"/> notification.
    /// Creates a <see cref="ProductValidationHistory"/> record and saves it to the database.
    /// </summary>
    /// <param name="notification">The event containing the product validation change details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Handle(ProductValidationChangedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            ProductValidationHistory history = new()
            {
                ProductId = notification.ProductId,
                UserId = notification.UserId,
                OldValue = notification.OldValue,
                NewValue = notification.NewValue,
                ModifyDate = notification.ModifyDate,
            };

            IBaseRepository<ProductValidationHistory>
                repository = unitOfWork.GetRepository<ProductValidationHistory>();
            await repository.AddAsync(history);

            await unitOfWork.SaveChangesAsync();
        }
        catch (Exception exception)
        {
            logger.UnhandledErrorLogger(exception);
        }
    }
}