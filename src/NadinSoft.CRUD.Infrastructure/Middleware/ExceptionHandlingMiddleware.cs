using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NadinSoft.CRUD.Application.Common.DTOs;

namespace NadinSoft.CRUD.Infrastructure.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Validation failed");

            context.Response.ContentType = "application/json";

            List<string> errors = ex.Errors.Select(e => $"{e.ErrorMessage}").ToList();
            ApiResponse<object> apiResponse = new(string.Join(" | ", errors));

            string json = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");

            context.Response.ContentType = "application/json";

            ApiResponse<object> apiResponse = new(ex.Message);


            string json = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(json);
        }
    }
}