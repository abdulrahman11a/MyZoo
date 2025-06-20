namespace Clinic.APIS.ProgramExtensions.ServiceExtensions
{
    public static class RateLimitingExtension
    {
        public static IServiceCollection AddRateLimitingServices(this IServiceCollection services)
        {
            #region Rate Limiting

            services.AddRateLimiter(options =>
            {
                options.AddSlidingWindowLimiter("BookingLimiter", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(10);
                    opt.SegmentsPerWindow = 5;
                    opt.PermitLimit = 3;
                    opt.QueueLimit = 1;
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                }).RejectionStatusCode = 429;

                options.AddFixedWindowLimiter("AccountLimiter", opt =>
                {
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.PermitLimit = 5;
                    opt.QueueLimit = 0;
                }).RejectionStatusCode = 429;
            });

            #endregion

            return services;
        }
    }

}
