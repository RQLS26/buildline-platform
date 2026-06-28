using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
///     Provides startup helpers for safe database initialization in container environments.
/// </summary>
internal static class DatabaseBootstrapper
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
