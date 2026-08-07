using System.ComponentModel.DataAnnotations;
using FiftyNine.EfCore.InterceptorDemo.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Pages.Admin;

public class ProductModel(DemoDbContext dbContext) : PageModel
{
    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    public int CategoryId { get; set; }

    [BindProperty]
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? LastModifiedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }

    public async Task<IActionResult> OnGetAsync(int categoryId, int id)
    {
        var product = await dbContext.Set<Product>().FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }

        Id = product.Id;
        CategoryId = categoryId;
        Name = product.Name;
        LastModifiedBy = product.LastModifiedBy;
        LastModifiedAt = product.LastModifiedAt;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var product = await dbContext.Set<Product>().FirstOrDefaultAsync(p => p.Id == Id);
        if (product is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            LastModifiedBy = product.LastModifiedBy;
            LastModifiedAt = product.LastModifiedAt;
            return Page();
        }

        product.Name = Name;
        await dbContext.SaveChangesAsync();

        return RedirectToPage("/Admin/Category", new { id = CategoryId });
    }
}
