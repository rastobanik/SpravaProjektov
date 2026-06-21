namespace SpravaProjektovAPI.Application.Logins
{
    public interface ILoginService
    {
        Task<string?> LoginAsync(string username, string password);
    }
}
