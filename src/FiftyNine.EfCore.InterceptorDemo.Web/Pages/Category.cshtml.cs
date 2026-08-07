using FiftyNine.EfCore.InterceptorDemo.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Pages;

[AllowAnonymous]
public class CategoryModel(DemoDbContext dbContext) : PageModel
{
    public ProductCategory? Category { get; set; }
    public Product[] Products { get; set; } = [];

    public Task OnGetAsync(int id)
     => GetData_v2(id);

    private async Task GetData_v1(int id)
    {
        Category = await dbContext.Set<ProductCategory>()
            .FirstOrDefaultAsync(c => c.Id == id);
        if (Category != null)
        {
            Products = await dbContext.Set<Product>()
                .Where(p => EF.Property<int>(p, "CategoryId") == id)
                .OrderBy(p => p.Name)
                .ToArrayAsync();
        }
    }
    private async Task GetData_v2(int id)
    {
        Category = await dbContext.Set<ProductCategory>().FirstOrDefaultAsync(c => c.Id == id);
        if (Category != null)
        {
            Products = await Category.GetProductsAsync();
        }
    }
}
