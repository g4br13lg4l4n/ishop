namespace Core.Specifications;
using Core.Entities;

public class TypeListSpecification : BaseSpecification<Product, string>
{
    public TypeListSpecification()
    {
        AddSelect(p => p.Type);
        SetIsDistinct();
    }
}
