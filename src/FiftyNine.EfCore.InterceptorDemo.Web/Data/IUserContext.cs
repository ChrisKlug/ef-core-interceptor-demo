namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

public interface IUserContext
{
    string? Name { get; }
    Tenants? CurrentTenant { get; }
}

public class HttpContextUserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUsers _users;

    public HttpContextUserContext(IHttpContextAccessor httpContextAccessor, IUsers users)
    {
        _httpContextAccessor = httpContextAccessor;
        _users = users;
    }

    public string? Name => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    public Tenants? CurrentTenant => _httpContextAccessor.HttpContext?.User?.FindFirst("Tenant")?.Value is string tenant
                ? Enum.Parse<Tenants>(tenant)
                : null;
}