using Microsoft.Extensions.Logging;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.LoginApplicationUser;

/// <summary>
/// Provides logging helpers for <see cref="LoginApplicationUserRequestHandler"/> operations.
/// </summary>
public static class LoginApplicationUserLogger
{
    /// <summary>
    /// Defines the log message for unhandled errors during the login process.
    /// </summary>
    private static readonly Action<ILogger, Exception?> UnhandledError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(UnhandledError)),
            "Unhandled error occurred while logging-in request.");

    /// <summary>
    /// Logs an unhandled exception that occurred while processing a login request.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="LoginApplicationUserRequestHandler"/>.</param>
    /// <param name="exception">The exception to log.</param>
    public static void UnhandledErrorLogger(
        this ILogger<LoginApplicationUserRequestHandler> logger,
        Exception exception) =>
        UnhandledError(logger, exception);
}