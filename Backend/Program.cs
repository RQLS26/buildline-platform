using Buildline.Platform.Categories.Application.Internal.QueryServices;
using Buildline.Platform.Categories.Application.QueryServices;
using Buildline.Platform.Categories.Domain.Repositories;
using Buildline.Platform.Categories.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Iam.Application.CommandServices;
using Buildline.Platform.Iam.Application.Internal.CommandServices;
using Buildline.Platform.Iam.Application.Internal.OutboundServices;
using Buildline.Platform.Iam.Application.Internal.QueryServices;
using Buildline.Platform.Iam.Application.QueryServices;
using Buildline.Platform.Iam.Domain.Repositories;
using Buildline.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;
using Buildline.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using Buildline.Platform.Iam.Infrastructure.Tokens.Jwt.Services;
using Buildline.Platform.Materials.Application.CommandServices;
using Buildline.Platform.Materials.Application.Internal.CommandServices;
using Buildline.Platform.Materials.Application.Internal.QueryServices;
using Buildline.Platform.Materials.Application.QueryServices;
using Buildline.Platform.Materials.Domain.Repositories;
using Buildline.Platform.Materials.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Profiles.Application.CommandServices;
using Buildline.Platform.Profiles.Application.Internal.CommandServices;
using Buildline.Platform.Profiles.Application.Internal.QueryServices;
using Buildline.Platform.Profiles.Application.QueryServices;
using Buildline.Platform.Profiles.Domain.Repositories;
using Buildline.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Projects.Application.Internal.QueryServices;
using Buildline.Platform.Projects.Application.QueryServices;
using Buildline.Platform.Projects.Domain.Repositories;
using Buildline.Platform.Projects.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Resources.Shared;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi;
using ProblemDetailsFactory = Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddDataAnnotationsLocalization();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in the configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Shared/Resources");
builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<ProblemDetailsFactory>();
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Buildline Platform API",
            Version = "v1",
            Description = "Backend Web Services for Buildline construction logistics workflows.",
            Contact = new OpenApiContact
            {
                Name = "RQLS Team",
                Email = "contact@buildline.com"
            }
        });
    options.EnableAnnotations();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryQueryService, CategoryQueryService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMaterialCommandService, MaterialCommandService>();
builder.Services.AddScoped<IMaterialQueryService, MaterialQueryService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectQueryService, ProjectQueryService>();

var app = builder.Build();

app.UseGlobalExceptionHandler();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontendPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/api/v1/health", () => Results.Ok(new { status = "Healthy", service = "Buildline Platform API" }))
    .WithName("GetHealth");
app.MapControllers();

app.Run();
