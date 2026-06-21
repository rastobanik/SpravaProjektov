using SpravaProjektovUI.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace SpravaProjektovUI.Infrastructure
{
    public class LoginApiClient
    {
        private readonly HttpClient _http;
                
        public LoginApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<LoginResponseDto?> LoginAsync(string username, string password)
        {
            var request = new LoginRequestDto
            {
                Username = username,
                Password = password
            };

            var response = await _http.PostAsJsonAsync("login", request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return null;

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadAsStringAsync();

            return new LoginResponseDto()
            {
                Username = username,
                Token = token
            };            
        }
    }
}
