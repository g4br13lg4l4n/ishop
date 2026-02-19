using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> ListAllAsync();
    // this is to get the entity with the specification

    Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    // this is to get the entity with the specification
    // this is to get the list of entities with the specification
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
  
    
    // this is to get the entity with the specification
    // this is to get the list of entities with the specification
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
    // this is to get the list of entities with the specification
    // this is to get the count of entities with the specification
    Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);
    
    // this is to get the count of entities with the specification
    Task<int> CountAsync(ISpecification<T> spec);

    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);

    Task<bool> SaveAllAsync();

    bool Exists(int id);

}
