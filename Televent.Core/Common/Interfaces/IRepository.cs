namespace Televent.Core.Common.Interfaces;

public interface IRepository<TEntity, TId>
{
    Task<TEntity?> GetByIdAsync(TId id);
    IAsyncEnumerable<TEntity> ListAllAsync();
    Task InsertAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}