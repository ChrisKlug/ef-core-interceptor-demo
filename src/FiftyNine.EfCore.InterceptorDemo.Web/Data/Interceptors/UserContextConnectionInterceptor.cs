using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data.Interceptors;

public class UserContextConnectionInterceptor(
    IUserContext userContext,
    Func<Tenants, string> getConnectionString
) : IDbConnectionInterceptor
{
    public InterceptionResult<DbConnection> ConnectionCreating(
        ConnectionCreatingEventData eventData, 
        InterceptionResult<DbConnection> result
    )
    {
        result = InterceptionResult<DbConnection>.SuppressWithResult(
            new SqlConnection(getConnectionString(userContext.CurrentTenant ?? Tenants.Default))
        );
        return result;
    }
}
