using Microsoft.Extensions.Logging;

namespace NadinSoft.CRUD.Application.Services.ProductService.Command.CreateProduct;

/// <summary>
/// Provides logging helpers for <see cref="CreateProductRequestHandler"/> operations.
/// </summary>
public static class CreateProductLogger
{
    /// <summary>
    /// Defines the log message for unhandled errors during product creation.
    /// </summary>
    /// <value>The <see cref="Action{ILogger, Exception}"/> representing the log definition.</value>
    private static readonly Action<ILogger<CreateProductRequestHandler>, Exception?> UnhandledError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(UnhandledError)),
            "Unhandled error occurred while processing product create request.");

    /// <summary>
    /// Defines the log message for a duplicate product detection.
    /// </summary>
    private static readonly Action<ILogger<CreateProductRequestHandler>, string, DateTime, Exception?>
        DuplicateProduct =
            LoggerMessage.Define<string, DateTime>(
                LogLevel.Warning,
                new EventId(0, nameof(DuplicateProduct)),
                "Duplicate product detected: ManufactureEmail={Email}, ProduceDate={Date}");

    /// <summary>
    /// Logs an unhandled exception that occurred during product creation.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="CreateProductRequestHandler"/>.</param>
    /// <param name="exception">The exception to log.</param>
    public static void UnhandledErrorLogger(
        this ILogger<CreateProductRequestHandler> logger,
        Exception exception) =>
        UnhandledError(logger, exception);

    /// <summary>
    /// Logs a warning when a duplicate product is detected.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="CreateProductRequestHandler"/>.</param>
    /// <param name="email">The manufacture email of the duplicate product.</param>
    /// <param name="date">The produce date of the duplicate product.</param>
    public static void DuplicateProductLogger(
        this ILogger<CreateProductRequestHandler> logger,
        string email,
        DateTime date) =>
        DuplicateProduct(logger, email, date, null);
}