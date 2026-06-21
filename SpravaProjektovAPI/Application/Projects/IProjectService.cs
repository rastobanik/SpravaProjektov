using SpravaProjektovAPI.Model;

namespace SpravaProjektovAPI.Application.Projects
{
    public interface IProjectService
    {
        Task<Project> CreateProjectAsync(Project projectDto);

        Task DeleteProjectAsync(string key);
        
        Task<List<Project>> GetAllProjectsAsync();
        
        Task<Project?> GetProjectByIdAsync(string key);
        
        Task UpdateProjectAsync(Project projectDto);
    }
}
