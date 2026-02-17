using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

// this is the base specification class that all specifications will inherit from
public class BaseSpecification<T>(Expression<Func<T, bool>> criteria) : ISpecification<T>
{
    // this is the criteria for the specification
    public Expression<Func<T, bool>>? Criteria => criteria;
    // this is the includes for the specification
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
    // this is the order by for the specification
    public Expression<Func<T, object>> OrderBy { get; private set; } = null!;
    // this is the order by descending for the specification
    public Expression<Func<T, object>> OrderByDescending { get; private set; } = null!;
    // this is the take for the specification
    public int Take { get; private set; }
    // this is the skip for the specification
    public int Skip { get; private set; }
    // this is the is paging enabled for the specification
    public bool IsPagingEnabled { get; private set; }

    protected void AddOrderBy(Expression<Func<T, object>> includeExpression)
    {
        OrderBy = includeExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> includeExpression)
    {
        OrderByDescending = includeExpression;
    }
    
    
}
