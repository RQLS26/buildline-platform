using System.Reflection;
using Buildline.Platform.Analytics.Application.Acl;
using Buildline.Platform.Analytics.Application.CommandServices;
using Buildline.Platform.Analytics.Application.Internal.CommandServices;
using Buildline.Platform.Analytics.Application.Internal.QueryServices;
using Buildline.Platform.Analytics.Application.QueryServices;
using Buildline.Platform.Analytics.Domain.Repositories;
using Buildline.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Analytics.Interfaces.Acl;
using Buildline.Platform.Communication.Application.CommandServices;
using Buildline.Platform.Communication.Application.Internal.CommandServices;
using Buildline.Platform.Communication.Application.Internal.QueryServices;
using Buildline.Platform.Communication.Application.QueryServices;
using Buildline.Platform.Communication.Domain.Repositories;
using Buildline.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Delivery.Application.CommandServices;
using Buildline.Platform.Delivery.Application.Internal.CommandServices;
using Buildline.Platform.Delivery.Application.Internal.OutboundServices;
using Buildline.Platform.Delivery.Application.Internal.QueryServices;
using Buildline.Platform.Delivery.Application.QueryServices;
using Buildline.Platform.Delivery.Domain.Repositories;
using Buildline.Platform.Delivery.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
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
using Buildline.Platform.Inventory.Application.CommandServices;
using Buildline.Platform.Inventory.Application.Internal.CommandServices;
using Buildline.Platform.Inventory.Application.Internal.QueryServices;
using Buildline.Platform.Inventory.Application.QueryServices;
using Buildline.Platform.Inventory.Domain.Repositories;
using Buildline.Platform.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Procurement.Application.CommandServices;
using Buildline.Platform.Procurement.Application.Internal.CommandServices;
using Buildline.Platform.Procurement.Application.Internal.OutboundServices;
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
using Buildline.Platform.Requisition.Application.CommandServices;
using Buildline.Platform.Requisition.Application.Internal.CommandServices;
using Buildline.Platform.Requisition.Application.Internal.OutboundServices;
using Buildline.Platform.Requisition.Application.Internal.QueryServices;
using Buildline.Platform.Requisition.Application.QueryServices;
using Buildline.Platform.Requisition.Domain.Repositories;
using Buildline.Platform.Requisition.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Resources.Shared;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Seeding;
using Buildline.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using Buildline.Platform.Suppliers.Application.CommandServices;
using Buildline.Platform.Suppliers.Application.Internal.CommandServices;
using Buildline.Platform.Suppliers.Application.Internal.QueryServices;
using Buildline.Platform.Suppliers.Application.QueryServices;
using Buildline.Platform.Suppliers.Domain.Repositories;
using Buildline.Platform.Suppliers.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using ProblemDetailsFactory = Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails.ProblemDetailsFactory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

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
var tokenSigningKey = TokenSigningKeyFactory.CreateFromSecret(tokenSecret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = tokenSigningKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManageUsers", policy => policy.RequireRole("owner", "admin"));
    options.AddPolicy("OwnUsers", policy => policy.RequireRole("owner"));
});

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

var supportedCultures = new[] { "en", "en-US", "es", "es-PE" };
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

    await DatabaseBootstrapper.EnsureCompatibilitySchemaAsync(dbContext, logger);

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
    /// <summary>
    ///     Adds narrowly scoped compatibility columns required by newer application code when a
    ///     pre-migration Railway database already contains Buildline tables without EF history.
    /// </summary>
    /// <param name="dbContext">Application database context used to execute metadata and DDL commands.</param>
    /// <param name="logger">Startup logger used to report non-destructive compatibility actions.</param>
    /// <returns>A task that completes once every required compatibility column exists.</returns>
    /// <remarks>
    ///     This method intentionally does not run the full EF migration pipeline against legacy
    ///     databases, because those environments may already contain manually created tables and
    ///     seeded demo evidence. It only creates columns that are known to be backward-compatible
    ///     and required by mapped entities loaded during authentication.
    /// </remarks>
    public static async Task EnsureCompatibilitySchemaAsync(AppDbContext dbContext, ILogger logger)
    {
        if (!await ColumnExistsAsync(dbContext, "users", "two_factor_enabled"))
        {
            await ExecuteNonQueryAsync(dbContext, "ALTER TABLE `users` ADD COLUMN `two_factor_enabled` tinyint(1) NOT NULL DEFAULT 0");
            logger.LogInformation("Added missing compatibility column users.two_factor_enabled.");
        }

        if (!await ColumnExistsAsync(dbContext, "users", "company_id"))
        {
            await ExecuteNonQueryAsync(dbContext, "ALTER TABLE `users` ADD COLUMN `company_id` int NULL");
            logger.LogInformation("Added missing compatibility column users.company_id.");
        }

        if (!await ColumnExistsAsync(dbContext, "users", "membership_status"))
        {
            await ExecuteNonQueryAsync(dbContext, "ALTER TABLE `users` ADD COLUMN `membership_status` varchar(20) NOT NULL DEFAULT 'active'");
            logger.LogInformation("Added missing compatibility column users.membership_status.");
        }

        var defaultCompanyId = await GetFirstProfileIdAsync(dbContext);
        if (defaultCompanyId is null)
            logger.LogWarning("No company profile was found while preparing compatibility company scope columns.");

        var companyScopedOperationalTables = new[]
        {
            "projects",
            "budgets",
            "materials",
            "requisitions",
            "purchase_orders",
            "quotations",
            "inventory_items",
            "deliveries",
            "suppliers",
            "supplier_incidents",
            "messages"
        };

        foreach (var tableName in companyScopedOperationalTables)
        {
            if (await TableExistsAsync(dbContext, tableName) &&
                !await ColumnExistsAsync(dbContext, tableName, "company_id"))
            {
                if (defaultCompanyId is null)
                    continue;

                await ExecuteNonQueryAsync(dbContext, $"ALTER TABLE `{tableName}` ADD COLUMN `company_id` int NOT NULL DEFAULT {defaultCompanyId.Value}");
                logger.LogInformation("Added missing compatibility column {TableName}.company_id with default company {CompanyId}.", tableName, defaultCompanyId.Value);
            }
        }

        await EnsureDefaultCompanyMembershipAsync(dbContext, logger, defaultCompanyId);

        await EnsureSingleOwnerAsync(dbContext, logger);
    }

    /// <summary>
    ///     Resolves the first persisted company profile used to scope legacy rows that predate tenancy.
    /// </summary>
    /// <param name="dbContext">Application database context used to execute the profile lookup.</param>
    /// <returns>The first profile identifier when available; otherwise <c>null</c>.</returns>
    private static async Task<int?> GetFirstProfileIdAsync(AppDbContext dbContext)
    {
        const string firstProfileQuery = "SELECT `id` FROM `profiles` ORDER BY `id` LIMIT 1";
        var firstProfileId = await ExecuteScalarAsync(dbContext, firstProfileQuery);
        return firstProfileId is null || firstProfileId == DBNull.Value
            ? null
            : Convert.ToInt32(firstProfileId);
    }

    /// <summary>
    ///     Assigns legacy users to the resolved company profile when old Railway data predates tenancy fields.
    /// </summary>
    /// <param name="dbContext">Application database context used to execute compatibility SQL.</param>
    /// <param name="logger">Startup logger used to report data corrections.</param>
    /// <param name="defaultCompanyId">Resolved company profile identifier used for legacy membership rows.</param>
    /// <returns>A task that completes once legacy membership rows are normalized.</returns>
    private static async Task EnsureDefaultCompanyMembershipAsync(AppDbContext dbContext, ILogger logger, int? defaultCompanyId)
    {
        if (defaultCompanyId is null)
            return;

        const string assignLegacyUsersCommand = """
            UPDATE `users`
            SET `company_id` = @companyId,
                `membership_status` = COALESCE(NULLIF(`membership_status`, ''), 'active')
            WHERE `company_id` IS NULL
            """;

        var affectedRows = await ExecuteNonQueryAsync(dbContext, assignLegacyUsersCommand, command =>
        {
            var companyParameter = command.CreateParameter();
            companyParameter.ParameterName = "@companyId";
            companyParameter.Value = defaultCompanyId.Value;
            command.Parameters.Add(companyParameter);
        });

        if (affectedRows > 0)
            logger.LogInformation("Assigned {AffectedRows} legacy user(s) to the default company profile.", affectedRows);
    }

    /// <summary>
    ///     Repairs legacy IAM data so the company account keeps exactly one project owner.
    /// </summary>
    /// <param name="dbContext">Application database context used to query and update IAM users.</param>
    /// <param name="logger">Startup logger used to report compatibility data corrections.</param>
    /// <returns>A task that completes once duplicated owner rows, if any, have been normalized.</returns>
    /// <remarks>
    ///     Older frontend builds allowed promoting regular users to <c>owner</c>. The current IAM
    ///     command service already blocks new duplicate owners; this startup guard only repairs
    ///     existing Railway data by preserving <c>admin@buildline.com</c> when present, or otherwise
    ///     preserving the earliest owner row and demoting the rest to <c>admin</c>.
    /// </remarks>
    private static async Task EnsureSingleOwnerAsync(AppDbContext dbContext, ILogger logger)
    {
        const string ownerKeeperQuery = """
            SELECT `id`
            FROM `users`
            WHERE LOWER(`role`) = 'owner'
            ORDER BY CASE WHEN LOWER(`email`) = 'admin@buildline.com' THEN 0 ELSE 1 END, `id`
            LIMIT 1
            """;

        var ownerKeeperId = await ExecuteScalarAsync(dbContext, ownerKeeperQuery);
        if (ownerKeeperId is null || ownerKeeperId == DBNull.Value)
            return;

        const string demoteDuplicateOwnersCommand = """
            UPDATE `users`
            SET `role` = 'admin'
            WHERE LOWER(`role`) = 'owner'
              AND `id` <> @ownerKeeperId
            """;

        var affectedRows = await ExecuteNonQueryAsync(dbContext, demoteDuplicateOwnersCommand, command =>
        {
            var ownerKeeperParameter = command.CreateParameter();
            ownerKeeperParameter.ParameterName = "@ownerKeeperId";
            ownerKeeperParameter.Value = ownerKeeperId;
            command.Parameters.Add(ownerKeeperParameter);
        });

        if (affectedRows > 0)
            logger.LogWarning("Demoted {AffectedRows} duplicated owner user(s) to admin during IAM compatibility repair.", affectedRows);
    }

    /// <summary>
    ///     Checks whether a column exists in the active MySQL database.
    /// </summary>
    /// <param name="dbContext">Application database context used to obtain the active connection.</param>
    /// <param name="tableName">Database table name to inspect.</param>
    /// <param name="columnName">Database column name to inspect.</param>
    /// <returns><c>true</c> when the column exists; otherwise <c>false</c>.</returns>
    private static async Task<bool> ColumnExistsAsync(AppDbContext dbContext, string tableName, string columnName)
    {
        const string query = """
            SELECT COUNT(*)
            FROM information_schema.columns
            WHERE table_schema = DATABASE()
              AND table_name = @tableName
              AND column_name = @columnName
            """;

        var result = await ExecuteScalarAsync(dbContext, query, command =>
        {
            var tableParameter = command.CreateParameter();
            tableParameter.ParameterName = "@tableName";
            tableParameter.Value = tableName;
            command.Parameters.Add(tableParameter);

            var columnParameter = command.CreateParameter();
            columnParameter.ParameterName = "@columnName";
            columnParameter.Value = columnName;
            command.Parameters.Add(columnParameter);
        });

        return Convert.ToInt32(result) > 0;
    }

    /// <summary>
    ///     Checks whether a table exists in the active MySQL database.
    /// </summary>
    /// <param name="dbContext">Application database context used to obtain the active connection.</param>
    /// <param name="tableName">Database table name to inspect.</param>
    /// <returns><c>true</c> when the table exists; otherwise <c>false</c>.</returns>
    private static async Task<bool> TableExistsAsync(AppDbContext dbContext, string tableName)
    {
        const string query = """
            SELECT COUNT(*)
            FROM information_schema.tables
            WHERE table_schema = DATABASE()
              AND table_name = @tableName
            """;

        var result = await ExecuteScalarAsync(dbContext, query, command =>
        {
            var tableParameter = command.CreateParameter();
            tableParameter.ParameterName = "@tableName";
            tableParameter.Value = tableName;
            command.Parameters.Add(tableParameter);
        });

        return Convert.ToInt32(result) > 0;
    }

    /// <summary>
    ///     Executes a scalar database command while preserving the caller's connection state.
    /// </summary>
    /// <param name="dbContext">Application database context used to obtain the active connection.</param>
    /// <param name="commandText">SQL command text to execute.</param>
    /// <param name="configureCommand">Optional callback used to add command parameters.</param>
    /// <returns>The first column of the first row returned by the command.</returns>
    private static async Task<object?> ExecuteScalarAsync(
        AppDbContext dbContext,
        string commandText,
        Action<System.Data.Common.DbCommand>? configureCommand = null)
    {
        var connection = dbContext.Database.GetDbConnection();
        var shouldCloseConnection = connection.State == System.Data.ConnectionState.Closed;
        if (shouldCloseConnection)
            await connection.OpenAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = commandText;
            configureCommand?.Invoke(command);
            return await command.ExecuteScalarAsync();
        }
        finally
        {
            if (shouldCloseConnection)
                await connection.CloseAsync();
        }
    }

    /// <summary>
    ///     Executes a non-query database command while preserving the caller's connection state.
    /// </summary>
    /// <param name="dbContext">Application database context used to obtain the active connection.</param>
    /// <param name="commandText">SQL command text to execute.</param>
    /// <param name="configureCommand">Optional callback used to add command parameters.</param>
    /// <returns>The number of affected rows reported by the database provider.</returns>
    private static async Task<int> ExecuteNonQueryAsync(
        AppDbContext dbContext,
        string commandText,
        Action<System.Data.Common.DbCommand>? configureCommand = null)
    {
        var connection = dbContext.Database.GetDbConnection();
        var shouldCloseConnection = connection.State == System.Data.ConnectionState.Closed;
        if (shouldCloseConnection)
            await connection.OpenAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = commandText;
            configureCommand?.Invoke(command);
            return await command.ExecuteNonQueryAsync();
        }
        finally
        {
            if (shouldCloseConnection)
                await connection.CloseAsync();
        }
    }
}
