namespace Clinic.APIS.ProgramExtensions.ServiceExtensions
{
    public static class Connections
    {
        public static IServiceCollection AddDatabaseAndHangfireConnections(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Contexts
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
                });
            });

            services.AddDbContext<AppIdentityDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            // Hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            return services;
        }
    }
}
