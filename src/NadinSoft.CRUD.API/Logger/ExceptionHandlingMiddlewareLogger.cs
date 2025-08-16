using NadinSoft.CRUD.API.Middleware;

namespace NadinSoft.CRUD.API.Logger;

/// <summary>
/// Provides logging helpers for <see cref="ExceptionHandlingMiddleware"/> to log validation and unhandled exceptions.
/// </summary>
public static class ExceptionHandlingMiddlewareLogger
{
    /// <summary>
    /// Defines the log message for validation failures.
    /// </summary>
    private static readonly Action<ILogger<ExceptionHandlingMiddleware>, Exception?> ValidationFailed =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(ValidationFailed)),
            "Validation failed.");

    /// <summary>
    /// Defines the log message for unhandled exceptions.
    /// </summary>
    private static readonly Action<ILogger<ExceptionHandlingMiddleware>, Exception?> UnhandledException =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(UnhandledException)),
            "An unhandled exception occurred.");

    /// <summary>
    /// Logs a validation failure exception.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="ExceptionHandlingMiddleware"/>.</param>
    /// <param name="exception">The exception that caused the validation failure.</param>
    public static void ValidationFailedLogger(
        this ILogger<ExceptionHandlingMiddleware> logger,
        Exception exception) =>
        ValidationFailed(logger, exception);

    /// <summary>
    /// Logs an unhandled exception.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="ExceptionHandlingMiddleware"/>.</param>
    /// <param name="exception">The unhandled exception.</param>
    public static void UnhandledExceptionLogger(
        this ILogger<ExceptionHandlingMiddleware> logger,
        Exception exception) =>
        UnhandledException(logger, exception);
}