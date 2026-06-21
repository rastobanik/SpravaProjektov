using SpravaProjektovAPI.Model;

namespace SpravaProjektovAPI.Application.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> CreateProjectAsync(Project projectDto)
        {
            return await _projectRepository.AddAsync(projectDto);
        }

        public async Task DeleteProjectAsync(string key)
        {
           await _projectRepository.DeleteAsync(key);
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(string key)
        {
            return await _projectRepository.GetByIdAsync(key);
        }

        public async Task UpdateProjectAsync(Project projectDto)
        {
           await _projectRepository.UpdateAsync(projectDto);
        }
    }
}
