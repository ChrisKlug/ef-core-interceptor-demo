namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

public interface IAmChangeTracked
{
    public string LastModifiedBy { set; }
    public DateTimeOffset LastModifiedAt { set; }
}

public class Product : IAmChangeTracked
{
    private ProductCategory? category;

    private Product() { } // EF Core requires a parameterless constructor

    public Product(string name, decimal price, ProductCategory category)
    {
        Name = name;
        Price = price;
        this.category = category ?? throw new ArgumentNullException(nameof(category));
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string LastModifiedBy { get; private set; } = null!;
    public DateTimeOffset LastModifiedAt { get; private set; }

    string IAmChangeTracked.LastModifiedBy { set => LastModifiedBy = value; }
    DateTimeOffset IAmChangeTracked.LastModifiedAt { set => LastModifiedAt = value; }
}