using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Mvc;
using SpravaProjektovAPI.Application.Projects;
using SpravaProjektovAPI.Infrastructure;
using SpravaProjektovAPI.Validators;

namespace SpravaProjektovAPI.EndPoints
{
    public static class ProjectEndpoints
    {

        public static void MapProjectEndpoints(this IEndpointRouteBuilder app, ApiVersionSet apiVersionsSet)
        {
            var group = app.MapGroup("/api/v{version:apiVersion}/projects")
                .WithApiVersionSet(apiVersionsSet) // pridanie skupiny endpointov pre projekty, ktora bude mat verziu API
                .WithTags("Projects");

            _ = group.MapGet("/", GetAllProjects)
                .HasApiVersion(ApiVersions.V1); // verzia API je V1, pretoze nacitanie vsetkych projektov je dostupne len pre verziu V1

            _ = group.MapGet("/{key}", GetProjectById)
                .IsApiVersionNeutral(); // verzia API je neutralna, pretoze nacitanie projektu podla ID je dostupne pre vsetky verzie API

            _ = group.MapPost("/", CreateProject)
                .AddEndpointFilter<ValidationFilter<ProjectRequest>>() // pridanie endpoint filtra pre validaciu DTO
                .IsApiVersionNeutral(); // verzia API je neutralna, pretoze vytvorenie projektu je dostupne pre vsetky verzie API

            _ = group.MapPut("/", UpdateProject)
                .AddEndpointFilter<ValidationFilter<ProjectRequest>>() // pridanie endpoint filtra pre validaciu DTO
                .IsApiVersionNeutral(); // verzia API je neutralna, pretoze update projektu je dostupne pre vsetky verzie API

            _ = group.MapDelete("/{key}", DeleteProject)
                .IsApiVersionNeutral(); // verzia API je neutralna, pretoze mazanie projektu je dostupne pre vsetky verzie API

            _ = group.MapGet("/", GetAllProjects2)
                .HasApiVersion(ApiVersions.V2); // verzia API je V2, pretoze nacitanie vsetkych projektov je dostupne len pre verziu V2
        }

        // nacitanie vsetkych projektov
        private static async Task<IResult> GetAllProjects([FromServices] IProjectService projectService)
        {
            // chyba mapovanie na DTO, pretoze sa vracia priamo entita Project
            var projects = await projectService.GetAllProjectsAsync();
            return Results.Ok(projects);
        }

        private static async Task<IResult> GetAllProjects2([FromServices] IProjectService projectService, [FromServices] ProjectMapper mapper, [FromServices] ILogger<Program> logger)
        {
            var projects = await projectService.GetAllProjectsAsync();

            var project2 = mapper.MapToResponseList(projects);
            return Results.Ok(project2);
        }

        // nacitanie jedneho projektu
        private static async Task<IResult> GetProjectById(string key, [FromServices] IProjectService projectService, [FromServices] ProjectMapper mapper, [FromServices] ILogger<Program> logger)
        {
            var project = await projectService.GetProjectByIdAsync(key);
            if (project == null)
            {
                return Results.NotFound();
            }

            // chyba mapovanie na DTO, pretoze sa vracia priamo entita Project
            return Results.Ok(project);
        }

        /// <summary>
        /// Vytvorenie projektu
        /// </summary>
        /// <param name="projectRequest">DTO reprezentujúce projekt</param>
        /// <param name="projectService">Služba na správu projektov</param>
        /// <param name="mapper">Mapper na konverziu medzi DTO a entitami</param>
        /// <returns></returns>
        private static async Task<IResult> CreateProject([FromBody]ProjectRequest projectRequest, [FromServices] IProjectService projectService, [FromServices] ProjectMapper mapper, [FromServices] ILogger<Program> logger)
        {

            var project = mapper.MapProjectDtoToProject(projectRequest);
            var createdProject = await projectService.CreateProjectAsync(project);

            var projectResponse = mapper.MapProjectToProjectResponse(createdProject);           
            return Results.Created($"/api/projects/{createdProject.Id}", projectResponse);
        }

        // update projektu
        private static async Task<IResult> UpdateProject([FromBody] ProjectRequest projectDto, [FromServices] IProjectService projectService, [FromServices] ProjectMapper mapper, [FromServices] ILogger<Program> logger)
        {
            var project = mapper.MapProjectDtoToProject(projectDto);
            await projectService.UpdateProjectAsync(project);
            
            return Results.NoContent();
        }

        // zmazanie projektu
        private static async Task<IResult> DeleteProject(string key, [FromServices] IProjectService projectService, [FromServices] ILogger<Program> logger)
        {
            await projectService.DeleteProjectAsync(key);            
            return Results.NoContent();
        }
    }
}
