using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    // this is to create the specification
    public ProductSpecification(ProductSpecParams specParams) : base(x =>
    (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand)) &&
    (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type))) // filter by brand and type
    {
        // this is to apply the paging to the specification 
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        // this is to add the order by to the specification
        switch (specParams.Sort)
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
