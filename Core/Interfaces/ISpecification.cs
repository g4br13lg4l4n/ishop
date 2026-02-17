using System.Linq.Expressions;
using Core.Entities;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    Expression<Func<T, object>> OrderBy { get; }
    Expression<Func<T, object>> OrderByDescending { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }

    bool IsDistinct { get; }
}

public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>> Select { get; }
}
