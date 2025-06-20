namespace Clinic.APIS.ProgramExtensions.ServiceExtensions
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityAndCoreServices(this IServiceCollection services)
        {
            // Email token provider
            services.AddTransient<EmailTokenProvider<AppUser>>();

            // Identity options
            services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.ProviderMap.Add("Email", new TokenProviderDescriptor(typeof(EmailTokenProvider<AppUser>)));
                options.SignIn.RequireConfirmedEmail = true;
            });

            // Core application services
            services.AddInfrastructure(); // Register UnitOfWork and Repositories
            services.AddAutoMapper(typeof(ApplyMapping));
            services.AddScoped<CapacityHelper>();
            services.AddScoped<CapacityCleanupJob>();

            // Identity framework
            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<AppIdentityDBContext>();

            return services;
        }
    }
}
