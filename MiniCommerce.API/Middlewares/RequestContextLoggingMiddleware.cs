using Serilog.Context;

namespace MiniCommerce.API.Middlewares;

public class RequestContextLoggingMiddleware : IMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";
    
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Request.Headers.TryGetValue(CorrelationIdHeaderName, 
            out var correlationId);
        
        correlationId = correlationId.FirstOrDefault() ?? context.TraceIdentifier;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            return next(context);
        }
    }
}