namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

public interface IUsers
{
    Task<User?> Validate(string username, string password);
}

public class InMemoryUsers : IUsers
{
    private readonly Dictionary<string, User> _users = new Dictionary<string, User>(StringComparer.CurrentCultureIgnoreCase)
    {
        ["Kiter"] = new User("1", "Kiter", "password123", Tenants.Kite),
        ["Winger"] = new User("2", "Winger", "password123", Tenants.Wingfoil),
        ["Admin"] = new User("3", "Admin", "password123", Tenants.Default, true)
    };

    public Task<User?> Validate(string username, string password)
    {
        return Task.FromResult(
            _users.TryGetValue(username, out var user) && user.Password == password ? user : null);
    }
}
