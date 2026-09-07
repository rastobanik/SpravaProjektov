using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;
using SpravaProjektovAPI.Application.Logins;
using SpravaProjektovAPI.Validators;

namespace SpravaProjektovAPI.EndPoints
{
    public static class LoginEndpoints
    {
        public static void MapLoginEndpoints(this IEndpointRouteBuilder app, ApiVersionSet apiVersionsSet)
        {

            var versionedGroup = app.MapGroup("/api/v{version:apiVersion}")
                 .WithApiVersionSet(apiVersionsSet);

            // Define login-related endpoints here
            versionedGroup.MapPost("/login", Login).                
                WithTags("Login")
                .AddEndpointFilter<ValidationFilter<LoginRequest>>()
                .IsApiVersionNeutral();
        }

        private static async Task<IResult> Login(LoginRequest loginRequest, [FromServices] ILoginService loginService, ILogger<Program> logger)
        {
            var result = await loginService.LoginAsync(loginRequest.Username, loginRequest.Password);
            if (string.IsNullOrEmpty(result))
            {
                return Results.Unauthorized();
            }

            return Results.Ok(result);
        }
    }
}