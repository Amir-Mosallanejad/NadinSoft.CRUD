using Microsoft.Extensions.Logging;

namespace NadinSoft.CRUD.Application.Events.ProductValidationChanged;

/// <summary>
/// Provides extension methods for <see cref="ILogger{T}"/> to log errors
/// related to <see cref="ProductValidationChangedEventHandler"/> processing.
/// </summary>
public static class ProductValidationChangedLogger
{
    /// <summary>
    /// A pre-defined <see cref="Action{T1, T2}"/> used for logging unhandled exceptions
    /// that occur during the processing of <see cref="ProductValidationChangedEventHandler"/> events.
    /// Uses <see cref="LoggerMessage.Define"/> for high-performance logging.
    /// </summary>
    private static readonly Action<ILogger<ProductValidationChangedEventHandler>, Exception?> UnhandledError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(UnhandledError)),
            "Unhandled error occurred while processing ProductValidationHistory event.");

    /// <summary>
    /// Logs an unhandled exception that occurred during processing of a
    /// <see cref="ProductValidationChangedEvent"/> by <see cref="ProductValidationChangedEventHandler"/>.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger{T}"/> instance to write the log to.</param>
    /// <param name="exception">The exception that was thrown.</param>
    public static void UnhandledErrorLogger(
        this ILogger<ProductValidationChangedEventHandler> logger,
        Exception exception) =>
        UnhandledError(logger, exception);
}