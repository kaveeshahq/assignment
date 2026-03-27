using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FlowDesk.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly FlowDeskDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(FlowDeskDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entities)
    {
        DbSet.UpdateRange(entities);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate == null)
            return await DbSet.CountAsync();

        return await DbSet.CountAsync(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.AnyAsync(predicate);
    }
}
