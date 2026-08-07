using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

public class DemoDbContext : DbContext
{
    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
    {
    }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.ToTable("ProductCategories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            
            entity.Ignore(e => e.ProductProvider);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Price).IsRequired();
            entity.Property<int>("CategoryId").HasColumnName("ProductCategoryId").IsRequired();
            entity.HasOne<ProductCategory>("category")
                .WithMany()
                .HasForeignKey("CategoryId");
        });
    }
}