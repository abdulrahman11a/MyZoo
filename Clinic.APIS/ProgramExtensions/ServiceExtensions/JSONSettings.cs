using static Clinic.infrastructure.Configuration.ScheduleTime_Config;
namespace Clinic.APIS.ProgramExtensions.ServiceExtensions
{
    public static class JSONSettings
    {
        public static IServiceCollection AddCustomJsonSettings(this IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.Converters.Add(new EnumMemberConverter<ScheduleGroup>());
                    options.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter());
                });

            return services;
        }
    }
}
