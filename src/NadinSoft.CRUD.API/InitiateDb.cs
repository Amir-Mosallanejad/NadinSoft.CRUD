using Microsoft.EntityFrameworkCore;
using NadinSoft.CRUD.Infrastructure.Data;

namespace NadinSoft.CRUD.API;

public static class InitiateDb
{
    public static void MigrateDb(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        const int maxRetries = 10;
        int retry = 0;

        while (retry < maxRetries)
        {
            try
            {
                if (context.Database.IsRelational())
                {
                    context.Database.Migrate();
                }

                logger.LogInformation("Database migrated successfully.");
                break;
            }
            catch (Exception)
            {
                retry++;
                logger.LogWarning("Database not ready. Retrying in {Delay}s... Attempt {Retry}/{MaxRetries}", 5,
                    retry, maxRetries);
                Thread.Sleep(5000);
            }
        }

        if (retry == maxRetries)
        {
            logger.LogError("Could not connect to the database after {MaxRetries} attempts.", maxRetries);
            throw new Exception("Failed to migrate the database.");
        }
    }
}