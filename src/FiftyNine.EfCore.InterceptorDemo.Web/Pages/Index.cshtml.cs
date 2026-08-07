using FiftyNine.EfCore.InterceptorDemo.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Pages;

[AllowAnonymous]
public class IndexModel(DemoDbContext dbContext, IUserContext userContext) : PageModel
{
    public List<ProductCategory> Categories { get; set; } = [];

    public ShopTheme Theme => ShopThemes.For(userContext.CurrentTenant);

    public async Task OnGetAsync()
    {
        Categories = await dbContext.Set<ProductCategory>()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToPage("/Login");
    }
}
