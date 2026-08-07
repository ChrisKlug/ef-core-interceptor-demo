namespace FiftyNine.EfCore.InterceptorDemo.Web.Data;

public record ShopTheme(string Name, string Tagline, string Emoji, string Primary, string PrimaryDark, string Bg);

public static class ShopThemes
{
    public static readonly Dictionary<Tenants, ShopTheme> All = new()
    {
        [Tenants.Default] = new(
            "Nine Beach Shop",
            "Sun, sand, and everything in between.",
            "🏖️",
            "#b9762f",
            "#8f5a20",
            "#faf3e8"),
        [Tenants.Kite] = new(
            "Nine Kite Shop",
            "Catch the wind. Ride the waves.",
            "🪁",
            "#1f6fb2",
            "#154f82",
            "#eaf4fb"),
        [Tenants.Wingfoil] = new(
            "Nine Wingfoil Shop",
            "Wing up. Foil out.",
            "🌊",
            "#0f9488",
            "#0b6d64",
            "#e8f7f5"),
    };

    public static ShopTheme For(Tenants? tenant) => All[tenant ?? Tenants.Default];
}
