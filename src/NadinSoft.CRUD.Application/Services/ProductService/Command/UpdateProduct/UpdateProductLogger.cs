using Microsoft.Extensions.Logging;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.UpdateProduct;

/// <summary>
/// Provides logging helpers for <see cref="UpdateProductRequestHandler"/> operations.
/// </summary>
public static class UpdateProductLogger
{
    /// <summary>
    /// Defines the log message for unhandled errors during product update.
    /// </summary>
    private static readonly Action<ILogger<UpdateProductRequestHandler>, Exception?> UnhandledError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(UnhandledError)),
            "Unhandled error occurred while processing product update request.");

    /// <summary>
    /// Defines the log message for a product not found scenario.
    /// </summary>
    private static readonly Action<ILogger<UpdateProductRequestHandler>, Guid, Exception?> ProductNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(0, nameof(ProductNotFound)),
            "Product not found with Id: {Id}");

    /// <summary>
    /// Defines the log message for unauthorized update attempts.
    /// </summary>
    private static readonly Action<ILogger<UpdateProductRequestHandler>, string, Guid, string, Exception?>
        UnauthorizedUpdateAttempt =
            LoggerMessage.Define<string, Guid, string>(
                LogLevel.Warning,
                new EventId(0, nameof(UnauthorizedUpdateAttempt)),
                "Unauthorized update attempt by user {UserId} on product {ProductId} created by {CreatorId}.");

    /// <summary>
    /// Logs an unhandled exception that occurred during product update.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="UpdateProductRequestHandler"/>.</param>
    /// <param name="exception">The exception to log.</param>
    public static void UnhandledErrorLogger(
        this ILogger<UpdateProductRequestHandler> logger,
        Exception exception) =>
        UnhandledError(logger, exception);

    /// <summary>
    /// Logs a warning when a product is not found for update.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="UpdateProductRequestHandler"/>.</param>
    /// <param name="productId">The ID of the product that was not found.</param>
    public static void ProductNotFoundLogger(
        this ILogger<UpdateProductRequestHandler> logger,
        Guid productId) =>
        ProductNotFound(logger, productId, null);

    /// <summary>
    /// Logs a warning when a user attempts to update a product they do not own.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="UpdateProductRequestHandler"/>.</param>
    /// <param name="userId">The ID of the user attempting the update.</param>
    /// <param name="productId">The ID of the product being updated.</param>
    /// <param name="creatorId">The ID of the product creator.</param>
    public static void UnauthorizedUpdateAttemptLogger(
        this ILogger<UpdateProductRequestHandler> logger,
        string userId,
        Guid productId,
        string creatorId) =>
        UnauthorizedUpdateAttempt(logger, userId, productId, creatorId, null);
}