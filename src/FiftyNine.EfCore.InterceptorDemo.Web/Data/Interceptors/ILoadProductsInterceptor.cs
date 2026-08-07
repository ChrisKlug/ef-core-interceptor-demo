using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FiftyNine.EfCore.InterceptorDemo.Web.Data.Interceptors;

public class ILoadProductsInterceptor : IMaterializationInterceptor
{
    public object InitializedInstance(MaterializationInterceptionData materializationData, object entity)
    {
        if (entity is ILoadProducts productsLoader)
        {
            productsLoader.GetProducts = category => materializationData.Context.Set<Product>()
                .Where(p => EF.Property<int>(p, "CategoryId") == category.Id)
                .OrderBy(p => p.Name)
                .ToArrayAsync();
        }
        return entity;
    }

    public static ILoadProductsInterceptor Instance { get; } = new ILoadProductsInterceptor();
}
