using Serilog;
using SpravaProjektovAPI.Application.Logins;
using SpravaProjektovAPI.Application.Projects;
using SpravaProjektovAPI.EndPoints;
using SpravaProjektovAPI.ExceptionHandling;
using SpravaProjektovAPI.Model;
using System.Linq.Expressions;
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
    builder.Services.AddScoped<IProjectService, ProjectService>();
    builder.Services.AddScoped<ILoginService, LoginService>(login =>
    {
        var userName = builder.Configuration.GetValue<string>("User")
       ?? throw new InvalidOperationException("UserName not configured.");

        var password = builder.Configuration.GetValue<string>("Password")
       ?? throw new InvalidOperationException("Password not configured.");

        return new LoginService(userName, password);
    });
    
    builder.Services.AddScoped<IProjectRepository, XMLRepository>(provider =>
    {
        var relativePath = builder.Configuration.GetValue<string>("ProjectDataFilePath")
        ?? throw new InvalidOperationException("ProjectDataFilePath not configured.");

        var filePath = Path.Combine(builder.Environment.ContentRootPath, relativePath);

        return new XMLRepository(filePath);
    });


    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // add global error handler services
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseSerilogRequestLogging();

    app.UseExceptionHandler();

    // Map the endpoints for login and project management
    app.MapProjectEndpoints();
    app.MapLoginEndpoints();

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
