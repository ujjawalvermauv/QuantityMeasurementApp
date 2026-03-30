using System.Text.Json;
using QuantityMeasurementApp.Api.Contracts;
using QuantityMeasurementApp.Business.Exceptions;

namespace QuantityMeasurementApp.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (QuantityMeasurementException ex)
        {
            _logger.LogWarning(ex, "Business exception was thrown by quantity API.");
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Quantity Measurement Error", ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument for quantity API request.");
            await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in quantity API.");
            await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error", ex.Message);
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, int statusCode, string error, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var payload = new ErrorResponseDto
        {
            Timestamp = DateTime.UtcNow,
            Status = statusCode,
            Error = error,
            Message = message,
            Path = context.Request.Path
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
