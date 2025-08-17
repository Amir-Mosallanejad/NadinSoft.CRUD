using Microsoft.Extensions.Logging;

namespace NadinSoft.CRUD.Application.Services.ProductService.Query.GetAllProducts;

/// <summary>
/// Provides logging helpers for <see cref="GetAllProductsRequestHandler"/> operations.
/// </summary>
public static class GetAllProductsLogger
{
    /// <summary>
    /// Defines the log message for unhandled errors during retrieving all products.
    /// </summary>
    private static readonly Action<ILogger<GetAllProductsRequestHandler>, Exception?> UnhandledError =
        LoggerMessage.Define(
            LogLevel.Error,
            new EventId(0, nameof(UnhandledError)),
            "Error occurred while retrieving all products.");

    /// <summary>
    /// Logs an unhandled exception that occurred while retrieving all products.
    /// </summary>
    /// <param name="logger">The logger instance for <see cref="GetAllProductsRequestHandler"/>.</param>
    /// <param name="exception">The exception to log.</param>
    public static void UnhandledErrorLogger(
        this ILogger<GetAllProductsRequestHandler> logger,
        Exception exception) =>
        UnhandledError(logger, exception);
}