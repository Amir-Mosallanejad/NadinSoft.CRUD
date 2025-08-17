using Microsoft.Extensions.Logging;

namespace NadinSoft.CRUD.Application.Services.ApplicationUserService.Command.RegisterApplicationUser;

/// <summary>
/// Provides logging helpers for <see cref="RegisterApplicationUserRequestHandler"/> operations.
/// </summary>
public static class RegisterApplicationUserLogger
{
    /// <summary>
    /// Defines the log message for unhandled errors during the registration process.
    /// </summary>
    private static readonly Action<ILogger<RegisterApplicationUserRequestHandler>, Exception?> UnhandledError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(UnhandledError)),
            "Unhandled error occurred while registration request.");

    /// <summary>
    /// Defines the log message for a registration attempt with an existing email.
    /// </summary>
    private static readonly Action<ILogger<RegisterApplicationUserRequestHandler>, string, Exception?>
        ExistingEmailError =
            LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(0, nameof(ExistingEmailError)),
                "Registration attempt failed: User with email {Email} already exists.");

    /// <summary>
    /// Defines the log message for a registration attempt that failed with validation errors.
    /// </summary>
    private static readonly Action<ILogger<RegisterApplicationUserRequestHandler>, string, string, Exception?>
        RegistrationAttemptFailed =
            LoggerMessage.Define<string, string>(
                LogLevel.Warning,
                new EventId(0, nameof(RegistrationAttemptFailed)),
                "User registration failed for email {Email}: {Errors}");

    /// <summary>
    /// Logs an unhandled exception that occurred during user registration.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="RegisterApplicationUserRequestHandler"/>.</param>
    /// <param name="exception">The exception to log.</param>
    public static void UnhandledErrorLogger(
        this ILogger<RegisterApplicationUserRequestHandler> logger,
        Exception exception) =>
        UnhandledError(logger, exception);

    /// <summary>
    /// Logs a registration attempt that failed because the email already exists.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="RegisterApplicationUserRequestHandler"/>.</param>
    /// <param name="email">The email address that caused the failure.</param>
    public static void ExistingEmailErrorLogger(
        this ILogger<RegisterApplicationUserRequestHandler> logger,
        string email) =>
        ExistingEmailError(logger, email, null);

    /// <summary>
    /// Logs a registration attempt that failed with validation errors.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="RegisterApplicationUserRequestHandler"/>.</param>
    /// <param name="email">The email address of the user attempting registration.</param>
    /// <param name="errors">The validation errors encountered during registration.</param>
    public static void RegistrationAttemptFailedLogger(
        this ILogger<RegisterApplicationUserRequestHandler> logger,
        string email,
        string errors) =>
        RegistrationAttemptFailed(logger, email, errors, null);
}