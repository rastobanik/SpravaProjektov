using Microsoft.AspNetCore.Mvc;
using SpravaProjektovAPI.Application.Logins;

namespace SpravaProjektovAPI.EndPoints
{
    public static class LoginEndpoints
    {
        public static void MapLoginEndpoints(this IEndpointRouteBuilder app)
        {
            // Define login-related endpoints here

            app.MapPost("/api/login", Login).WithTags("Login");
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