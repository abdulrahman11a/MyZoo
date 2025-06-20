namespace Clinic.APIS.ProgramExtensions.ServiceExtensions
{
    public static class RedisServiceExtension
    {
        public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration config)
        {
            #region Redis & Caching
            var redisConnection = config.GetConnectionString("Redis");
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddSingleton<IAppointmentsService, AppointmentsService>();
            #endregion


            #region Health Checks

            services.AddHealthChecks()
                .AddSqlServer(config.GetConnectionString("DefaultConnection"))
                .AddRedis(redisConnection);

            #endregion

            return services;
        }
    }

}
