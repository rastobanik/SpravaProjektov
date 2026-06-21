using SpravaProjektovUI.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace SpravaProjektovUI.Infrastructure
{
    public class ProjectApiClient
    {
        private readonly HttpClient _http;

        public ProjectApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProjectDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<ProjectDto>>("projects")
                   ?? [];
        }

        public async Task CreateAsync(ProjectDto project)
        {
            var response = await _http.PostAsJsonAsync("projects", project);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string id)
        {
            var response = await _http.DeleteAsync($"projects/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpadateAsync(ProjectDto project)
        {
            var response = await _http.PutAsJsonAsync("projects", project);
            response.EnsureSuccessStatusCode();
        }       
    }
}