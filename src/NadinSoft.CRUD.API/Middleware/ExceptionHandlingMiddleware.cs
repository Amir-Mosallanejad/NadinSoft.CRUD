// <copyright file="ExceptionHandlingMiddleware.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.API.Middleware;

using System.Text.Json;
using FluentValidation;
using NadinSoft.CRUD.Application.Common.DTOs;

/// <summary>
/// Middleware for handling exceptions and returning consistent API responses.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger used for logging exceptions.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
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
            await this.next(context);
        }
        catch (ValidationException ex)
        {
            this.logger.LogError(ex, "Validation failed.");

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
            this.logger.LogError(ex, "An unhandled exception occurred.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            ApiResponse<object> apiResponse =
                ApiResponse<object>.Fail("An unexpected error occurred. Please try again later.");

            string json = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(json);
        }
    }
}