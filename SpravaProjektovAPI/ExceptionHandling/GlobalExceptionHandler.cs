using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SpravaProjektovAPI.ExceptionHandling
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }


        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(
               exception, "Instance: {httpContext.TraceIdentifier}, Exception occurred: {Message}", httpContext.TraceIdentifier, exception.Message);

            var problem = new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred",
                Instance = httpContext.TraceIdentifier
            };

            httpContext.Response.StatusCode = problem.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}
