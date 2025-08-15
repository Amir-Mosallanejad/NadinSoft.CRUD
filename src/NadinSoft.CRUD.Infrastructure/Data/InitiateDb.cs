// <copyright file="InitiateDb.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NadinSoft.CRUD.Infrastructure.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides extension methods to initialize and migrate the database at application startup.
/// </summary>
public static class InitiateDb
{
    /// <summary>
    /// Applies pending migrations to the database. Retries up to 10 times if the database is not ready.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> used to create a service scope and access the <see cref="ApplicationDbContext"/>.</param>
    /// <exception cref="Exception">
    /// Thrown if the database could not be migrated after the maximum number of retries.
    /// </exception>
    public static void MigrateDb(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        ILogger<ApplicationDbContext>
            logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
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
                logger.LogWarning(
                    "Database not ready. Retrying in {Delay}s... Attempt {Retry}/{MaxRetries}",
                    5,
                    retry,
                    maxRetries);
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