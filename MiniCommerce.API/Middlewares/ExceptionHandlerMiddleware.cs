using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniCommerce.API.Abstractions.Behaviours;

namespace MiniCommerce.API.Middlewares;

public class ExceptionHandlerMiddleware : IExceptionHandler
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occured: {Message}", exception.Message);

        ExceptionDetails exceptionDetails = GetExceptionDetails(exception);

        var problemDetails = new ProblemDetails
        {
            Status = exceptionDetails.Status,
            Title = exceptionDetails.Title,
            Detail = exceptionDetails.Detail,
        };
        
        if (exceptionDetails.Errors is not null)
            problemDetails.Extensions["errors"] = exceptionDetails.Errors;
        
        httpContext.Response.StatusCode = exceptionDetails.Status;
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }

    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            ValidationException validation => new ExceptionDetails(
                StatusCodes.Status400BadRequest,
                "ValidationFailure",
                "Validation error",
                "One or more validation errors occurred.",
                validation.Errors),
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "InternalServerError",
                "Internal server error",
                "An unexpected error occurred.",
                null)
        };
    }
}

internal sealed record ExceptionDetails(
    int Status, 
    string Type,
    string Title,
    string Detail,
    IEnumerable<object?> Errors);