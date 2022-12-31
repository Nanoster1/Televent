using Microsoft.EntityFrameworkCore;
using Televent.Core.Common.Interfaces;

namespace Televent.Data.Common.Shared;

public class RepositoryBase<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class
{
    protected readonly DbContext Context;
    protected readonly DbSet<TEntity> Set;

    public RepositoryBase(DbContext context)
    {
        Context = context;
        Set = context.Set<TEntity>();
    }

    public Task DeleteAsync(TEntity entity)
    {
        Set.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await Set.FindAsync(id);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await Set.AddAsync(entity);
    }

    public IAsyncEnumerable<TEntity> ListAllAsync()
    {
        return Set.AsAsyncEnumerable();
    }

    public Task UpdateAsync(TEntity entity)
    {
        Set.Update(entity);
        return Task.CompletedTask;
    }
}