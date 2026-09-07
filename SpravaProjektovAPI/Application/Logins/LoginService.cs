namespace SpravaProjektovAPI.Application.Logins
{
    public class LoginService : ILoginService
    {
        public async Task<string?> LoginAsync(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                return "fake-jwt-token";
            }

            return string.Empty;
        }
    }
}
