namespace Clinic.APIS.ProgramExtensions.ServiceExtensions
{
    public static class EmailServiceExtension
    {
        public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration config)
        {
            #region Email Service
            services.Configure<MailSetting>(config.GetSection("EmailSetingByGmail"));
            services.AddScoped<IEmailService, EmailService>();
            return services;
            #endregion
        }
    }

}
