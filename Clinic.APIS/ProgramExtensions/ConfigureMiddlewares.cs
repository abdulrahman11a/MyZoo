namespace Clinic.APIS.ProgramExtensions
{
    public static class MiddlewareExtensions
    {
        public static WebApplication ConfigureMiddlewares(this WebApplication app)
        {
            app.MapHealthChecks("/health");
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();

            return app;
        }

        public static WebApplication ConfigureSwaggerUI(this WebApplication app)
        {
            // Swagger UI configuration
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });

            // Map Controllers and Hangfire Dashboard
            app.MapControllers();
            app.UseHangfireDashboard("/hangfire");

            #region Hangfire Recurring Jobs

            BackgroundJob.Enqueue<IEmailService>(service =>
                service.SendEmailToMultipleRecipientsAsync());

            using (var scopeHang = app.Services.CreateScope())
            {
                var recurringJobManager = scopeHang.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                var logger = scopeHang.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    logger.LogInformation("Registering Hangfire recurring job 're-add-capacity-job'...");
                    recurringJobManager.AddOrUpdate<CapacityCleanupJob>(
                        "re-add-capacity-job",
                        job => job.ReAddCapacityForExpiredAppointments(),
                        Cron.Daily);
                    logger.LogInformation("Hangfire recurring job 're-add-capacity-job' registered successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to register Hangfire recurring job 're-add-capacity-job'.");
                    throw;
                }
            }

            #endregion

            return app;
        }
    }
}
