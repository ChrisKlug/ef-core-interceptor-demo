using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data.Interceptors;

public class ChangeTrackingInterceptor(IUserContext userContext, TimeProvider timeProvider) : ISaveChangesInterceptor
{
    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is not null)
        {
            UpdateAuditProperties(context, userContext, timeProvider);
        }
        return ValueTask.FromResult(result);
    }
    
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context;
        if (context is not null)
        {
            UpdateAuditProperties(context, userContext, timeProvider);
        }
        return result;
    }

    private static void UpdateAuditProperties(DbContext context, IUserContext userContext, TimeProvider timeProvider)
    {
        var entries = context.ChangeTracker.Entries<Product>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Property("LastModifiedBy").CurrentValue = userContext.Name;
            entry.Property("LastModifiedAt").CurrentValue = timeProvider.GetUtcNow();
        }
    }
}
