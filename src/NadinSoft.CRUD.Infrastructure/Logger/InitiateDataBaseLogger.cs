using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Infrastructure.Data;

namespace NadinSoft.CRUD.Infrastructure.Logger;

/// <summary>
/// Provides logging helpers for <see cref="ApplicationDbContext"/> database initialization operations.
/// </summary>
public static class InitiateDataBaseLogger
{
    /// <summary>
    /// Defines the log message for a failed database connection after multiple retries.
    /// </summary>
    private static readonly Action<ILogger<ApplicationDbContext>, int, Exception?> DataBaseConnectionFail =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(0, nameof(DataBaseConnectionFail)),
            "Could not connect to the database after {MaxRetries} attempts.");

    /// <summary>
    /// Defines the log message when the database is not ready and a retry attempt is made.
    /// </summary>
    private static readonly Action<ILogger<ApplicationDbContext>, int, int, int, Exception?> DatabaseNotReady =
        LoggerMessage.Define<int, int, int>(
            LogLevel.Warning,
            new EventId(0, nameof(DatabaseNotReady)),
            "Database not ready. Retrying in {Delay}s... Attempt {Retry}/{MaxRetries}");

    /// <summary>
    /// Defines the log message for a successful database migration.
    /// </summary>
    private static readonly Action<ILogger<ApplicationDbContext>, Exception?> DatabaseMigrated =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(0, nameof(DatabaseMigrated)),
            "Database migrated successfully.");

    /// <summary>
    /// Logs a failed database connection after the specified number of maximum retries.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="ApplicationDbContext"/>.</param>
    /// <param name="maxRetries">The maximum number of connection attempts.</param>
    public static void DataBaseConnectionFailLogger(
        this ILogger<ApplicationDbContext> logger,
        int maxRetries) =>
        DataBaseConnectionFail(logger, maxRetries, null);

    /// <summary>
    /// Logs that the database is not ready and a retry is being attempted.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="ApplicationDbContext"/>.</param>
    /// <param name="delay">The delay in seconds before retrying.</param>
    /// <param name="retry">The current retry attempt number.</param>
    /// <param name="maxRetries">The maximum number of retry attempts.</param>
    public static void DatabaseNotReadyLogger(
        this ILogger<ApplicationDbContext> logger,
        int delay,
        int retry,
        int maxRetries) =>
        DatabaseNotReady(logger, delay, retry, maxRetries, null);

    /// <summary>
    /// Logs that the database migration completed successfully.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="ApplicationDbContext"/>.</param>
    public static void DatabaseMigratedLogger(this ILogger<ApplicationDbContext> logger) =>
        DatabaseMigrated(logger, null);
}