using FiftyNine.EfCore.InterceptorDemo.Web.Data;
using FiftyNine.EfCore.InterceptorDemo.Web.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web;

public class SeedDataService : IHostedService
{
    private readonly IConfiguration configuration;

    public SeedDataService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionStrings = configuration.GetSection("ConnectionStrings").GetChildren()
            .Where(c => !string.IsNullOrEmpty(c.Value))
            .Select(c => (Name: c.Key, ConnectionString: c.Value!))
            .ToList();

        var tasks = connectionStrings.Select(connectionString => {
            var tenant = Enum.TryParse<Tenants>(connectionString.Name, true, out var parsedTenant) ? parsedTenant : Tenants.Default;
            return AddSeedData(connectionString.ConnectionString, tenant);
        });
        await Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task AddSeedData(string connectionString, Tenants tenant)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.AddInterceptors(
            new ChangeTrackingInterceptor(
                new SeedDataUserContext("System", Tenants.Default),
                TimeProvider.System
            )
        );

        using var tenantDbContext = new DemoDbContext(optionsBuilder.Options);
        try
        {
            if (await tenantDbContext.Set<ProductCategory>().CountAsync() == 0)
            {
                var categories = SeedData.ProductCategories[tenant];
                foreach (var (category, products) in categories)
                {
                    await tenantDbContext.Set<ProductCategory>().AddAsync(category);
                    foreach (var (name, price) in products)
                    {
                        var product = category.AddProduct(name, price);
                        await tenantDbContext.Set<Product>().AddAsync(product);
                    }
                }
                await tenantDbContext.SaveChangesAsync();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding database '{tenant}': {ex.Message}");
            throw;
        }
    }

    private static class SeedData
    {
        public static readonly Dictionary<Tenants, List<(ProductCategory Category, (string Name, decimal Price)[] Products)>> ProductCategories = new()
        {
            { Tenants.Default, new List<(ProductCategory Category, (string Name, decimal Price)[] Products)> { 
                (new ProductCategory("Swimwear"), [
                    ("Swimsuit", 49.99m),
                    ("Bikini", 39.99m),
                    ("Board Shorts", 29.99m)
                ]),
                (new ProductCategory("Beach Toys"), [
                    ("Beach Ball", 9.99m),
                    ("Sandcastle Kit", 14.99m),
                    ("Frisbee", 7.99m)
                ]),
                (new ProductCategory("Kites"), [
                    ("Kite", 99.99m),
                    ("Stunt Kite", 149.99m),
                    ("Kite Lines", 29.99m)
                ])
            } },
            { Tenants.Kite, new List<(ProductCategory Category, (string Name, decimal Price)[] Products)> { 
                (new ProductCategory("Kites"), [
                    ("Slingshot RPM 12m", 2249.99m),
                    ("Slingshot RPM 9m", 2049.99m),
                    ("Slingshot Rally 12m", 2739.99m),
                    ("Slingshot Rally 8m", 2339.99m),
                ]),
                (new ProductCategory("Boards"), [
                    ("Slingshot Hover Glide 138", 1999.99m),
                    ("Slingshot Hover Glide 136", 1899.99m),
                    ("Slingshot Hover Glide 134", 1799.99m),
                ]),
                (new ProductCategory("Wetsuits"), [
                    ("O'Neill HyperFreak 5/4mm", 399.99m),
                    ("O'Neill HyperFreak 4/3mm", 379.99m),
                    ("O'Neill HyperFreak 3/2mm", 329.99m),
                    ("Mystic Majestic 5/4mm", 399.99m),
                    ("Mystic Majestic 4/3mm", 379.99m),
                    ("Mystic Majestic 3/2mm", 329.99m)
                ]),
                (new ProductCategory("Harnesses"), [
                    ("Mystic Warrior", 349.99m),
                    ("Mystic Aviator", 329.99m),
                    ("Dakine C-2", 299.99m),
                    ("Prolimit Edge", 329.99m)
                ]),
                (new ProductCategory("Safety Equipment"), [
                    ("Mystic MK8 Helmet", 149.99m),
                    ("Ozone EXO Helmet", 169.99m),
                    ("Ride Engine Universe Helmet", 189.99m),
                ]),
                (new ProductCategory("Spare Parts"), [
                    ("Kite Lines", 29.99m),
                    ("Duotone Pump", 49.99m),
                    ("Duotone Pump Large", 69.99m),
                    ("Kite Repair Kit", 19.99m)
                ]),
            } },
            { Tenants.Wingfoil, new List<(ProductCategory Category, (string Name, decimal Price)[] Products)> {
                (new ProductCategory("Wings"), [
                    ("Duotone Ventis 7", 1299m),
                    ("Duotone Ventis 9", 1399m),
                    ("F-One Unit 4.5", 999m),
                    ("F-One Unit 5.0", 1099m),
                    ("Reedin Supernatural 8.2", 1099m)
                ]),
                (new ProductCategory("Boards"), [
                    ("Duotone Skybrid", 899m),
                    ("F-One Trax HRD", 999m),
                    ("Starboard Hyper Nut", 1099m)
                ]),
                (new ProductCategory("Foils"), [
                    ("Axis Foils Carve 1000", 1499m),
                    ("Axys Foils Carve 1200", 1599m),
                    ("Moses Hydrofoil 1000", 1699m),
                    ("Moses Hydrofoil 1200", 1799m)
                ]),
                (new ProductCategory("Wetsuits"), [
                    ("O'Neill HyperFreak 5/4mm", 399.99m),
                    ("O'Neill HyperFreak 4/3mm", 379.99m),
                    ("O'Neill HyperFreak 3/2mm", 329.99m),
                    ("Mystic Majestic 5/4mm", 399.99m),
                    ("Mystic Majestic 4/3mm", 379.99m),
                    ("Mystic Majestic 3/2mm", 329.99m)
                ]),
                (new ProductCategory("Spare Parts"), [
                    ("T-Nuts", 29.99m),
                    ("Duotone Pump", 49.99m),
                    ("Duotone Pump Large", 69.99m)
                ])
            } }
        };
        
    }

    private class SeedDataUserContext(string name, Tenants tenant) : IUserContext
    {
        public string? Name => name;

        public Tenants? CurrentTenant => tenant;
    }
}