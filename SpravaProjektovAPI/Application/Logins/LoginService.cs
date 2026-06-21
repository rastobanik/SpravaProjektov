namespace SpravaProjektovAPI.Application.Logins
{
    public class LoginService : ILoginService
    {

        private readonly string _user;
        private readonly string _password;

        public LoginService(string userName, string password) 
        { 
            _user  = userName;
            _password = password;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {            
            if (username == _user && password == _password)
            {
                return "fake-jwt-token";
            }

            return string.Empty;
        }
    }
}
