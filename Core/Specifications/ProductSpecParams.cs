namespace Core.Specifications;

public class ProductSpecParams
{
    private List<string> _brands = [];

    public List<string> Brands
    {
        get => _brands; // example: "Nike,Adidas"
        set => _brands = value.SelectMany(x =>
            x.Split(',', StringSplitOptions.RemoveEmptyEntries)
        ).ToList(); // example: ["Nike", "Adidas"]
    }

    private List<string> _types = [];

    public List<string> Types
    {
        get => _types; // example: "Shoes,Clothes"
        set => _types = value.SelectMany(x =>
            x.Split(',', StringSplitOptions.RemoveEmptyEntries)
        ).ToList(); // example: ["Shoes", "Clothes"]
    }

    public string? Sort { get; set; }
}
