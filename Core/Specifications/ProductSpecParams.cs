namespace Core.Specifications;

public class ProductSpecParams
{

    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;
    public int _pageSize = 6;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    
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
