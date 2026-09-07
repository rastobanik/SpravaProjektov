using Riok.Mapperly.Abstractions;
using SpravaProjektovAPI.Application.Projects;
using SpravaProjektovAPI.Model;

namespace SpravaProjektovAPI.EndPoints
{
    [Mapper]
    public partial class ProjectMapper
    {
        public partial Project MapProjectDtoToProject(ProjectRequest projectDto);

        // Hlavná metóda pre mapovanie zoznamu
        public partial List<ProjectResponse2> MapToResponseList(List<Project> projects);

        // Automaticky vygenerované mapovanie projektu (Mapperly si samé prepojí Customer -> Customer)
        public partial ProjectResponse2 MapToResponse(Project project);

        // Vlastná implementácia: Ak je string prázdny alebo null, vráti null, inak vytvorí objekt
        private CustomerResponse? MapToCustomerResponse(string? customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                return null;
            }

            return new CustomerResponse { Customer = customerName };
        }

        public partial ProjectResponse MapProjectToProjectResponse(Project createdProject);
       
    }
}
