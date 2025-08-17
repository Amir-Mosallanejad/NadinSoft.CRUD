using FluentValidation;
using NadinSoft.CRUD.API.Logger;
using NadinSoft.CRUD.Application.Common.DTOs;
using System.Text.Json;

namespace NadinSoft.CRUD.API.Middleware;

/// <summary>
/// Middleware for handling exceptions and returning consistent API responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    /// <summary>
    /// The next middleware in the HTTP request pipeline.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Logger for logging exceptions and other information.
    /// </summary>
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger used for logging exceptions.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions during request processing.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.ValidationFailedLogger(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            List<string> errors = ex.Errors
                .Select(e => e.ErrorMessage)
                .ToList();

            ApiResponse<object> apiResponse = ApiResponse<object>.Fail(string.Join(" | ", errors));

            string json = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.UnhandledExceptionLogger(ex);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ApiResponse<object> apiResponse =
                ApiResponse<object>.Fail("An unexpected error occurred. Please try again later.");

            string json = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(json);
        }
    }
}