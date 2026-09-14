using System.Reflection;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data.Interceptors;

public class ProductsProviderInjectionInterceptor : IMaterializationInterceptor
{
    public InterceptionResult<object> CreatingInstance(MaterializationInterceptionData materializationData, InterceptionResult<object> result)
    {
        if (materializationData.EntityType.ClrType == typeof(ProductCategory))
        {
            var instance = Activator.CreateInstance(
                typeof(ProductCategory),
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                [ new ProductsProvider((DemoDbContext)materializationData.Context) ],
                null
            );
            return InterceptionResult<object>.SuppressWithResult(instance!);
        }
        return result;
    }

    public static ProductsProviderInjectionInterceptor Instance { get; } = new ProductsProviderInjectionInterceptor();
}
