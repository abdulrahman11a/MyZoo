namespace Clinic.APIS.ProgramExtensions
{
    public static class AllowDependencies
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var config = builder.Configuration;

            services.AddAuthenticationServices(config);
            services.AddDatabaseAndHangfireConnections(config);
            services.AddEmailServices(config);
            services.AddIdentityAndCoreServices();
            services.AddCustomJsonSettings();
            services.AddRateLimitingServices();
            services.AddRedisServices(config);
            services.AddSwaggerDocumentation();
            services.AddModelValidation();
            services.AddControllers();

            builder.Host.AddCustomSerilog();

            return services; // Return services for method chaining
        }
    }
}