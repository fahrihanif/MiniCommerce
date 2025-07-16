using Microsoft.EntityFrameworkCore;
using MiniCommerce.API.Data;

namespace MiniCommerce.API.Extensions;

public interface IMigrationExtension
{
    void ApplyMigrations();
}

public class MigrationExtension(ApplicationDbContext context, ILogger<MigrationExtension> logger)
    : IMigrationExtension
{
    public void ApplyMigrations()
    {
        try
        {
            logger.LogInformation("Checking for pending migrations...");

            if (context.Database.GetPendingMigrations().Any())
            {
                logger.LogInformation("Applying database migrations...");
                context.Database.Migrate();
                logger.LogInformation("Database migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("Database is up to date. No migrations to apply.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying migrations.");
        }
    }
}
