using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    // this is to get the query from the input query
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        // this is to get the query from the input query
        var query = inputQuery;

        // this is to apply the criteria to the query
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        // this is to apply the order by to the query
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }

        // this is to apply the order by descending to the query
        if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        // this is to apply the paging to the query
        if (specification.IsPagingEnabled)
        {
            query = query.Skip(specification.Skip).Take(specification.Take);
        }

        return query;
    }
}
