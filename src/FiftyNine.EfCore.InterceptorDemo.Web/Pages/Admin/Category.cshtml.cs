using FiftyNine.EfCore.InterceptorDemo.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Pages.Admin;

public class CategoryModel(DemoDbContext dbContext) : PageModel
{
    public ProductCategory? Category { get; set; }
    public Product[] Products { get; set; } = [];

    public async Task OnGetAsync(int id)
    {
        Category = await dbContext.Set<ProductCategory>().FirstOrDefaultAsync(c => c.Id == id);
        if (Category != null)
        {
            Products = await Category.GetProductsAsync();
        }
    }
}
