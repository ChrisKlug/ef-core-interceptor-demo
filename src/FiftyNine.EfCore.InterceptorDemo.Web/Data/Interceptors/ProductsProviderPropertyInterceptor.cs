using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data.Interceptors;

public class ProductsProviderPropertyInterceptor : IMaterializationInterceptor
{
    public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
    {
        if (entity is ProductCategory category)
        {
            category.ProductProvider = new ProductsProvider((DemoDbContext)materializationData.Context);
        }
        return entity;
    }

    public static ProductsProviderPropertyInterceptor Instance { get; } = new ProductsProviderPropertyInterceptor();
}