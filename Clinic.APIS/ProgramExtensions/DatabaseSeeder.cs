    namespace Clinic.APIS.ProgramExtensions
    {
        public static class DatabaseSeeder
        {
            public static async Task ApplyMigrationsAndSeedingAsync(this WebApplication app)
            {
                #region Database Migration & Seeding

                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;

                var dbContext = services.GetRequiredService<StoreDbContext>();
                var identityContext = services.GetRequiredService<AppIdentityDBContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Applying database migrations...");
                    await dbContext.Database.MigrateAsync();
                    // await identityContext.Database.MigrateAsync(); // Uncomment if needed
                    logger.LogInformation("Database migrations applied successfully.");

                    logger.LogInformation("Seeding initial data...");
                    await StoreContextDataSeed.DataSeedAsync(dbContext);
                    // await IdentityStoreDataseed.SeedAsync(services); // Uncomment if needed
                    logger.LogInformation("Data seeding completed successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during database migration or data seeding.");
                }

                #endregion
            }
        }
    }
