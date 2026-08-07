using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data.Interceptors;

public class ProductsProvider(DemoDbContext dbContext) : IProvideProducts
    {
        public Task<Product[]> GetProducts(ProductCategory category)
            => dbContext.Set<Product>()
                .Where(p => EF.Property<int>(p, "CategoryId") == category.Id)
                .OrderBy(p => p.Name)
                .ToArrayAsync();
    }