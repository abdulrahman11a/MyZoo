namespace Clinic.APIS.ProgramExtensions
{
    public static class HangfireJobInitializer
    {
        public static void RegisterHangfireRecurringJobs(this WebApplication app)
        {

            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

            try
            {
                logger.LogInformation("Registering Hangfire job: 're-add-capacity-job'");
                recurringJobManager.AddOrUpdate<CapacityCleanupJob>(
                    "re-add-capacity-job",
                    job => job.ReAddCapacityForExpiredAppointments(),
                    Cron.Daily);

                logger.LogInformation("Recurring job registered successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to register Hangfire recurring job.");
            }
        }
    }

}
