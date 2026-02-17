using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }
    // this is to get the count of entities with the specification
    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    public bool Exists(int id)
    {
        return context.Set<T>().Any(e => e.Id == id);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    // this is to get the entity with the specification
    public async Task<T?> GetEntityWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    // this is to get all the entities
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    // this is to get the list of entities with the specification
    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    // this is to save all the changes to the database
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    // this is to update the entity in the database
    public void Update(T entity)
    {
        // this is to attach the entity to the context
        context.Set<T>().Attach(entity);
        // this is to set the state of the entity to modified
        // this is to update the entity in the database
        context.Entry(entity).State = EntityState.Modified;
    }

    // this is to apply the specification to the query
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), spec);
    }
}
