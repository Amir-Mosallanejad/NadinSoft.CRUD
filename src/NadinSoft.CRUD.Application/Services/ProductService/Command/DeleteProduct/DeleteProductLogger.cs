using Microsoft.Extensions.Logging;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.DeleteProduct;

/// <summary>
/// Provides logging helpers for <see cref="DeleteProductRequestHandler"/> operations.
/// </summary>
public static class DeleteProductLogger
{
    /// <summary>
    /// Defines the log message for unhandled errors during product deletion.
    /// </summary>
    private static readonly Action<ILogger<DeleteProductRequestHandler>, Exception?> UnhandledError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(UnhandledError)),
            "Unhandled error occurred while processing product delete request.");

    /// <summary>
    /// Defines the log message for a product not found scenario.
    /// </summary>
    private static readonly Action<ILogger<DeleteProductRequestHandler>, Guid, Exception?> ProductNotFound =
        LoggerMessage.Define<Guid>(
            LogLevel.Warning,
            new EventId(0, nameof(ProductNotFound)),
            "Product not found with Id: {Id}");

    /// <summary>
    /// Defines the log message for unauthorized delete attempts.
    /// </summary>
    private static readonly Action<ILogger<DeleteProductRequestHandler>, string, Guid, string, Exception?>
        UnauthorizedDeleteAttempt =
            LoggerMessage.Define<string, Guid, string>(
                LogLevel.Warning,
                new EventId(0, nameof(UnauthorizedDeleteAttempt)),
                "Unauthorized delete attempt by user {UserId} on product {ProductId} created by {CreatorId}.");

    /// <summary>
    /// Logs an unhandled exception that occurred during product deletion.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="DeleteProductRequestHandler"/>.</param>
    /// <param name="exception">The exception to log.</param>
    public static void UnhandledErrorLogger(
        this ILogger<DeleteProductRequestHandler> logger,
        Exception exception) =>
        UnhandledError(logger, exception);

    /// <summary>
    /// Logs a warning when a product is not found for deletion.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="DeleteProductRequestHandler"/>.</param>
    /// <param name="productId">The ID of the product that was not found.</param>
    public static void ProductNotFoundLogger(
        this ILogger<DeleteProductRequestHandler> logger,
        Guid productId) =>
        ProductNotFound(logger, productId, null);

    /// <summary>
    /// Logs a warning when a user attempts to delete a product they do not own.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="DeleteProductRequestHandler"/>.</param>
    /// <param name="userId">The ID of the user attempting the deletion.</param>
    /// <param name="productId">The ID of the product being deleted.</param>
    /// <param name="creatorId">The ID of the product creator.</param>
    public static void UnauthorizedDeleteAttemptLogger(
        this ILogger<DeleteProductRequestHandler> logger,
        string userId,
        Guid productId,
        string creatorId) =>
        UnauthorizedDeleteAttempt(logger, userId, productId, creatorId, null);
}