public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDependencies(builder);
        
        var app = builder.Build();

        await app.ApplyMigrationsAndSeedingAsync();
        app.ConfigureMiddlewares();
        app.ConfigureSwaggerUI();
        app.RegisterHangfireRecurringJobs();
        app.Run();
    }
}