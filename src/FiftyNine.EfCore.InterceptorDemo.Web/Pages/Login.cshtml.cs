using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using FiftyNine.EfCore.InterceptorDemo.Web.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Pages;

[AllowAnonymous]
public class LoginModel(IUsers users) : PageModel
{

    [BindProperty]
    [Required]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await users.Validate(Username, Password);

        if (user is null)
        {
            ErrorMessage = "Invalid username or password.";
            return Page();
        }

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user.ToClaimsPrincipal());

        return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : "/");
    }
}
