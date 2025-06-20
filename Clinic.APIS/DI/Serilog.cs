namespace Clinic.APIS.DI
{
    public static class SerilogExtension
    {
        public static IHostBuilder SerilogConfig(this IHostBuilder host)
        {
            return host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
