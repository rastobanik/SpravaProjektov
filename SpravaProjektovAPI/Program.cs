using Asp.Versioning;
using Asp.Versioning.Builder;
using FluentValidation;
using Microsoft.Extensions.Options;
using Serilog;
using SpravaProjektovAPI.Application.Logins;
using SpravaProjektovAPI.Application.Projects;
using SpravaProjektovAPI.EndPoints;
using SpravaProjektovAPI.ExceptionHandling;
using SpravaProjektovAPI.Infrastructure;
using SpravaProjektovAPI.Model;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;


try
{
    var builder = WebApplication.CreateBuilder(args);

    // kedze je XML v 1250 kodovej stranke, tak treba pridat kodovanie
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    // load configuration for serilog from appsettings.json
    builder.Host.UseSerilog((ctx, loggerConfig) => loggerConfig.ReadFrom.Configuration(ctx.Configuration));

    Log.Information("Starting up application");

    // Add services to the container.

    // mrknut sa na scrutor - pomocou neho by sa dalo zaregistrovat vsetky service a repository naraz, ale nechce sa mi to riesit teraz
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<ILoginService, LoginService>();    
    builder.Services.AddScoped<IProjectRepository, XMLRepository>(provider =>
    {
        var relativePath = builder.Configuration.GetValue<string>("ProjectDataFilePath")
        ?? throw new InvalidOperationException("ProjectDataFilePath not configured.");

        var filePath = Path.Combine(builder.Environment.ContentRootPath, relativePath);

        return new XMLRepository(filePath);
    });

    _ = builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    _ = builder.Services.AddSingleton<ProjectMapper>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    _ = builder.Services.AddEndpointsApiExplorer();

    // Add Swagger and bind it to the versioned explorer
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    _ = builder.Services.AddSwaggerGen(options =>
    {
        // Tento filter automaticky skryje/predvyplní {version} parameter v Swaggeri
        options.OperationFilter<SwaggerDefaultValues>();
    });

    // add global error handler services
    _ = builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    _ = builder.Services.AddProblemDetails();

    // add API versioning
    _ = builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV"; // Naformátuje verziu (napr. v1, v2) do Swagger dropdownu
            options.SubstituteApiVersionInUrl = true; // Automaticky nahradí {version} v routach reálnym číslom
        });

    var app = builder.Build();

    ApiVersionSet apiVersionsSet = app.NewApiVersionSet()
    .HasApiVersion(ApiVersions.V1)
    .HasApiVersion(ApiVersions.V2)
    //.HasDeprecatedApiVersion(ApiVersions.V1)
    .Build();       

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler();

    // Map the endpoints for login and project management
    app.MapProjectEndpoints(apiVersionsSet);
    app.MapLoginEndpoints(apiVersionsSet);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            // Použitie zabudovanej metódy app.DescribeApiVersions()
            // zaručí presne tie skupiny, ktoré sú zaregistrované v trasách
            foreach (var desc in app.DescribeApiVersions())
            {
                var url = $"/swagger/{desc.GroupName}/swagger.json";
                var name = desc.GroupName.ToUpperInvariant(); // Vytvorí štítok "V1", "V2"

                options.SwaggerEndpoint(url, name);
            }
        });
    }

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
