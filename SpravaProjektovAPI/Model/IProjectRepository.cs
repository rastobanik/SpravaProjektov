namespace SpravaProjektovAPI.Model
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();

        Task<Project?> GetByIdAsync(string key);

        Task<Project> AddAsync(Project project);

        Task UpdateAsync(Project project);

        Task DeleteAsync(string key);
    }
}
