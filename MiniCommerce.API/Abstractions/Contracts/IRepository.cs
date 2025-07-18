namespace MiniCommerce.API.Contracts;

public interface IRepository<TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
}