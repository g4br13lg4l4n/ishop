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

        // this is to apply the distinct to the query
        if (specification.IsDistinct)
        {
            query = query.Distinct();
        }

        return query;
    }


    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> inputQuery, 
    ISpecification<T, TResult> spec)
    {
        
        // this is to apply the criteria to the query
        if (spec.Criteria != null)
        {
            inputQuery = inputQuery.Where(spec.Criteria);
        }

        // this is to apply the order by to the query
        if (spec.OrderBy != null)
        {
            inputQuery = inputQuery.OrderBy(spec.OrderBy);
        }

        // this is to apply the order by descending to the query
        if (spec.OrderByDescending != null)
        {
            inputQuery = inputQuery.OrderByDescending(spec.OrderByDescending);
        }

        // this is to apply the paging to the query
        if (spec.IsPagingEnabled)
        {
            inputQuery = inputQuery.Skip(spec.Skip).Take(spec.Take);
        }

        var selectQuery = inputQuery as IQueryable<TResult>;

        if (spec.Select != null)
        {
            selectQuery = inputQuery.Select(spec.Select);
        }

        // this is to apply the distinct to the query
        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }

        return selectQuery ?? inputQuery.Cast<TResult>();
    }
}
