using Buildline.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Seeding;
using Buildline.Platform.Suppliers.Application.Internal.CommandServices;
using Buildline.Platform.Suppliers.Application.CommandServices;
using Buildline.Platform.Procurement.Application.Internal.CommandServices;
using Buildline.Platform.Procurement.Application.Internal.OutboundServices;
using Buildline.Platform.Procurement.Application.CommandServices;
using Buildline.Platform.Inventory.Application.Internal.CommandServices;
using Buildline.Platform.Inventory.Application.CommandServices;
using Buildline.Platform.Delivery.Application.Internal.CommandServices;
using Buildline.Platform.Delivery.Application.Internal.OutboundServices;
using Buildline.Platform.Delivery.Application.CommandServices;
using Buildline.Platform.Communication.Application.Internal.CommandServices;
using Buildline.Platform.Communication.Application.CommandServices;
using Buildline.Platform.Analytics.Application.Internal.CommandServices;
using Buildline.Platform.Analytics.Application.CommandServices;
using System.Reflection;
using System.Text;
using Buildline.Platform.Analytics.Application.Internal.QueryServices;
using Buildline.Platform.Analytics.Application.QueryServices;
using Buildline.Platform.Analytics.Domain.Repositories;
using Buildline.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Inventory.Application.Internal.QueryServices;
using Buildline.Platform.Inventory.Application.QueryServices;
using Buildline.Platform.Inventory.Domain.Repositories;
using Buildline.Platform.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Communication.Application.Internal.QueryServices;
using Buildline.Platform.Communication.Application.QueryServices;
using Buildline.Platform.Communication.Domain.Repositories;
using Buildline.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Delivery.Application.Internal.QueryServices;
using Buildline.Platform.Delivery.Application.QueryServices;
using Buildline.Platform.Delivery.Domain.Repositories;
using Buildline.Platform.Delivery.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Buildline.Platform.Iam.Application.Acl;
using Buildline.Platform.Iam.Application.CommandServices;
using Buildline.Platform.Iam.Application.Internal.CommandServices;
using Buildline.Platform.Iam.Application.Internal.OutboundServices;
using Buildline.Platform.Iam.Application.Internal.QueryServices;
using Buildline.Platform.Iam.Application.QueryServices;
using Buildline.Platform.Iam.Domain.Repositories;
using Buildline.Platform.Iam.Infrastructure.Hashing.BCrypt.Services;
using Buildline.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using Buildline.Platform.Iam.Infrastructure.Tokens.Jwt.Configuration;
using Buildline.Platform.Iam.Infrastructure.Tokens.Jwt.Services;
using Buildline.Platform.Iam.Interfaces.Acl;
using Buildline.Platform.Requisition.Application.CommandServices;
using Buildline.Platform.Requisition.Application.Internal.CommandServices;
using Buildline.Platform.Requisition.Application.Internal.OutboundServices;
using Buildline.Platform.Requisition.Application.Internal.QueryServices;
using Buildline.Platform.Requisition.Application.QueryServices;
using Buildline.Platform.Requisition.Domain.Repositories;
using Buildline.Platform.Requisition.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Procurement.Application.Internal.QueryServices;
using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Domain.Repositories;
using Buildline.Platform.Procurement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Profiles.Application.Acl;
using Buildline.Platform.Profiles.Application.CommandServices;
using Buildline.Platform.Profiles.Application.Internal.CommandServices;
using Buildline.Platform.Profiles.Application.Internal.QueryServices;
using Buildline.Platform.Profiles.Application.QueryServices;
using Buildline.Platform.Profiles.Domain.Repositories;
using Buildline.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Profiles.Interfaces.Acl;
using Buildline.Platform.Analytics.Application.Acl;
using Buildline.Platform.Analytics.Interfaces.Acl;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Resources.Shared;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using Buildline.Platform.Suppliers.Application.Internal.QueryServices;
using Buildline.Platform.Suppliers.Application.QueryServices;
using Buildline.Platform.Suppliers.Domain.Repositories;
using Buildline.Platform.Suppliers.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.OpenApi;
using Microsoft.IdentityModel.Tokens;
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

var tokenSecret = Environment.ExpandEnvironmentVariables(
    builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>()?.Secret ?? string.Empty);
if (string.IsNullOrWhiteSpace(tokenSecret))
    throw new InvalidOperationException("JWT token secret is not set in TokenSettings.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

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

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
    if (File.Exists(xmlFilePath))
        options.IncludeXmlComments(xmlFilePath);

    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "JWT Authorization header using the Bearer scheme.",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document, null)] = []
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDemoDataSeeder, JsonDemoDataSeeder>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryQueryService, CategoryQueryService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IMaterialCommandService, MaterialCommandService>();
builder.Services.AddScoped<IMaterialQueryService, MaterialQueryService>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectQueryService, ProjectQueryService>();
builder.Services.AddScoped<IProjectsContextFacade, ProjectsContextFacade>();
builder.Services.AddScoped<IRequisitionRepository, RequisitionRepository>();
builder.Services.AddScoped<IRequisitionCommandService, RequisitionCommandService>();
builder.Services.AddScoped<IProjectReferenceService, ProjectReferenceService>();
builder.Services.AddScoped<IRequisitionQueryService, RequisitionQueryService>();
builder.Services.AddScoped<IPurchaseOrderRepository, PurchaseOrderRepository>();
builder.Services.AddScoped<IPurchaseOrderCommandService, PurchaseOrderCommandService>();
builder.Services.AddScoped<ISupplierDirectoryService, SupplierDirectoryService>();
builder.Services.AddScoped<IPurchaseOrderQueryService, PurchaseOrderQueryService>();
builder.Services.AddScoped<IQuotationRepository, QuotationRepository>();
builder.Services.AddScoped<IQuotationCommandService, QuotationCommandService>();
builder.Services.AddScoped<IQuotationQueryService, QuotationQueryService>();
builder.Services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
builder.Services.AddScoped<IInventoryItemCommandService, InventoryItemCommandService>();
builder.Services.AddScoped<IInventoryItemQueryService, InventoryItemQueryService>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryCommandService, DeliveryCommandService>();
builder.Services.AddScoped<IPurchaseOrderReferenceService, PurchaseOrderReferenceService>();
builder.Services.AddScoped<IDeliveryQueryService, DeliveryQueryService>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISupplierCommandService, SupplierCommandService>();
builder.Services.AddScoped<ISupplierQueryService, SupplierQueryService>();
builder.Services.AddScoped<ISupplierIncidentRepository, SupplierIncidentRepository>();
builder.Services.AddScoped<ISupplierIncidentCommandService, SupplierIncidentCommandService>();
builder.Services.AddScoped<ISupplierIncidentQueryService, SupplierIncidentQueryService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IBudgetCommandService, BudgetCommandService>();
builder.Services.AddScoped<IBudgetQueryService, BudgetQueryService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageCommandService, MessageCommandService>();
builder.Services.AddScoped<IMessageQueryService, MessageQueryService>();

builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseRequestLogging();

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

var swaggerFlag = Environment.GetEnvironmentVariable("ENABLE_SWAGGER");
var swaggerEnabled = app.Environment.IsDevelopment() ||
                     (string.IsNullOrWhiteSpace(swaggerFlag)
                         ? app.Configuration.GetValue<bool>("Swagger:Enabled")
                         : string.Equals(swaggerFlag, "true", StringComparison.OrdinalIgnoreCase));

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontendPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRequestAuthorization();
app.UseAuthorization();

app.MapGet("/api/v1/health", () => Results.Ok(new { status = "Healthy", service = "Buildline Platform API" }))
    .WithName("GetHealth");

using (var initScope = app.Services.CreateScope())
{
    var dbContext = initScope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = initScope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var hasExistingTables = await dbContext.Database.CanConnectAsync() &&
                            (await dbContext.Database.GetAppliedMigrationsAsync()).Any() is false &&
                            await DatabaseBootstrapper.HasApplicationTablesAsync(dbContext);

    if (hasExistingTables)
    {
        logger.LogWarning("Existing application tables were found without EF migration history. Startup will preserve the current schema and data.");
    }
    else
    {
        await dbContext.Database.MigrateAsync();
    }

    var demoDataSeeder = initScope.ServiceProvider.GetRequiredService<IDemoDataSeeder>();
    await demoDataSeeder.SeedAsync();
}

app.MapControllers();

app.Run();
/// <summary>
///     Provides startup helpers for safe database initialization in container environments.
/// </summary>
file static class DatabaseBootstrapper
{
    /// <summary>
    ///     Checks whether the current MySQL database already contains Buildline application tables.
    /// </summary>
    /// <param name="dbContext">Application database context used to obtain the active connection.</param>
    /// <returns><c>true</c> when at least one non-migration application table exists; otherwise <c>false</c>.</returns>
    public static async Task<bool> HasApplicationTablesAsync(AppDbContext dbContext)
    {
        const string query = """
            SELECT COUNT(*)
            FROM information_schema.tables
            WHERE table_schema = DATABASE()
              AND table_type = 'BASE TABLE'
              AND table_name <> '__EFMigrationsHistory'
            """;

        var connection = dbContext.Database.GetDbConnection();
        var shouldCloseConnection = connection.State == System.Data.ConnectionState.Closed;
        if (shouldCloseConnection)
            await connection.OpenAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = query;
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }
        finally
        {
            if (shouldCloseConnection)
                await connection.CloseAsync();
        }
    }
}
