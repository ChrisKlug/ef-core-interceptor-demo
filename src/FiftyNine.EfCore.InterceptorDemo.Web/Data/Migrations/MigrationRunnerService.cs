using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

public class MigrationRunnerService : IHostedService
{
    private readonly IConfiguration configuration;

    public MigrationRunnerService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionStrings = configuration.GetSection("ConnectionStrings").GetChildren()
            .Select(c => c.Value)
            .Where(c => !string.IsNullOrEmpty(c))
            .Cast<string>()
            .ToList();
        
        var tasks = connectionStrings.Select(connectionString => MigrateTenantDatabaseAsync(connectionString, cancellationToken));
        await Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task MigrateTenantDatabaseAsync(string connectionString, CancellationToken cancellationToken)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        using var tenantDbContext = new DemoDbContext(optionsBuilder.Options);
        try
        {
            await tenantDbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error ensuring database is created for connection string '{connectionString}': {ex.Message}");
            throw;
        }
    }
}