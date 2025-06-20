namespace Clinic.APIS.ProgramExtensions
{
    public static class LoggingExtension
    {
        public static IHostBuilder AddCustomSerilog(this IHostBuilder host)
        {
            #region Logging

            host.SerilogConfig();

            #endregion

            return host;
        }
    }
}