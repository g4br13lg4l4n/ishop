namespace Core.Specifications;
using Core.Entities;

public class BrandListSpecification : BaseSpecification<Product, string>
{
    public BrandListSpecification()
    {
        AddSelect(p => p.Brand);
        SetIsDistinct();
    }
}
