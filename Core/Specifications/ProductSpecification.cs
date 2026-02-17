using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    // this is to create the specification
    public ProductSpecification(string? brand, string? type, string? sort) : base(x =>
    (string.IsNullOrEmpty(brand) || x.Brand == brand) &&
    (string.IsNullOrEmpty(type) || x.Type == type))
    {
        // this is to add the order by to the specification
        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
        }
    }
    


   
}
