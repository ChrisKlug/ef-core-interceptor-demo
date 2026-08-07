using System.Security.Claims;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

public enum Tenants
{
    Kite,
    Wingfoil,
    Default
}

public record User(string UserId, string Username, string Password, Tenants Tenant, bool IsAdmin = false)
{
    public ClaimsPrincipal ToClaimsPrincipal()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, UserId),
            new(ClaimTypes.Name, Username),
            new("Tenant", Tenant.ToString()),
            new("IsAdmin", IsAdmin.ToString())
        };
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "Custom"));
    }
}
