using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace src.Middleware;

/// <summary>
/// Handles exceptions, not to be confused with regular errors.
/// </summary>
public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        (int statusCode, string? title) = MapExceptions(exception);
        
        Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Namespace,
                    Title = title,
                    Status = statusCode,
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    Extensions = new Dictionary<string, object?>
                    {
                        { "requestId", httpContext.TraceIdentifier },
                        { "traceId", activity?.Id }
                    }
                }
            });
    }

    private static (int StatusCodes, string? title) MapExceptions(Exception exception)
    {
        return exception switch
        {
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };
    }
}