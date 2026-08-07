using FiftyNine.EfCore.InterceptorDemo.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Pages.Admin;

public class IndexModel(DemoDbContext dbContext) : PageModel
{
    public List<ProductCategory> Categories { get; set; } = [];

    public async Task OnGetAsync()
    {
        Categories = await dbContext.Set<ProductCategory>()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }
}
