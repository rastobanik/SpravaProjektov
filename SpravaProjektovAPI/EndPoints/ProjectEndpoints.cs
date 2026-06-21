using Microsoft.AspNetCore.Mvc;
using SpravaProjektovAPI.Application.Projects;
using SpravaProjektovAPI.Model;

namespace SpravaProjektovAPI.EndPoints
{
    public static class ProjectEndpoints
    {

        public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/projects").WithTags("Projects");
            group.MapGet("/", GetAllProjects);
            group.MapGet("/{key}", GetProjectById);
            group.MapPost("/", CreateProject);
            group.MapPut("/", UpdateProject);
            group.MapDelete("/{key}", DeleteProject);
        }

        // nacitanie vsetkych projektov
        private static async Task<IResult> GetAllProjects([FromServices] IProjectService projectService)
        {
            var projects = await projectService.GetAllProjectsAsync();
            return Results.Ok(projects);
        }

        // nacitanie jedneho projektu
        private static async Task<IResult> GetProjectById(string key, [FromServices] IProjectService projectService)
        {
            var project = await projectService.GetProjectByIdAsync(key);
            if (project == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(project);
        }

        // pridanie projektu
        private static async Task<IResult> CreateProject([FromBody]Project projectDto, [FromServices] IProjectService projectService)
        {
            var createdProject = await projectService.CreateProjectAsync(projectDto);
            return Results.Created($"/api/projects/{createdProject.Id}", createdProject);
        }

        // update projektu
        private static async Task<IResult> UpdateProject([FromBody]Project projectDto, [FromServices] IProjectService projectService)
        {
            await projectService.UpdateProjectAsync(projectDto);
            
            return Results.NoContent();
        }

        // zmazanie projektu
        private static async Task<IResult> DeleteProject(string key, [FromServices] IProjectService projectService)
        {
            await projectService.DeleteProjectAsync(key);            
            return Results.NoContent();
        }
    }
}
