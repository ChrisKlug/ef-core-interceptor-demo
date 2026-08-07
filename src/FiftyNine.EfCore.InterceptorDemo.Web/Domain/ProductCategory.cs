namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

public interface IProvideProducts
{
    Task<Product[]> GetProducts(ProductCategory category);
}

public interface ILoadProducts
{
    Func<ProductCategory, Task<Product[]>>? GetProducts { get; set; }
}

public class ProductCategory : ILoadProducts
{
    private IProvideProducts? productProvider;

    private ProductCategory() { } // EF Core requires a parameterless constructor
    private ProductCategory(IProvideProducts productProvider)
    {
        this.productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
    }
    public ProductCategory(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public Product AddProduct(string name, decimal price)
    {
        return new Product(name, price, this);
    }

    public async Task<Product[]> GetProductsAsync()
    {
        if (productProvider != null)
        {
            return await productProvider.GetProducts(this);
        }
        else
        if (ProductProvider != null)
        {
            return await ProductProvider.GetProducts(this);
        }
        else if (((ILoadProducts)this).GetProducts != null)
        {
            return await ((ILoadProducts)this).GetProducts(this);
        }
        else
        {
            throw new InvalidOperationException("No product provider or loader is set.");
        }
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    
    public IProvideProducts? ProductProvider { get; set; }

    Func<ProductCategory, Task<Product[]>>? ILoadProducts.GetProducts { get; set; }
}
