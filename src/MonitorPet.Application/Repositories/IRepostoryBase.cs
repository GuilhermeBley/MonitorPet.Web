using MonitorPet.Core.Entity;

namespace MonitorPet.Application.Repositories;

public interface IRepositoryBase<TEntity, TModel, TId> : IRepositoryBase<TModel, TId>
    where TEntity : IEntity
    where TModel : class
{
    Task<TModel> Create(TEntity entity);
    Task<TModel?> DeleteByIdOrDefault(TId id);
    Task<TModel?> UpdateByIdOrDefault(TId id, TEntity entity);
}

public interface IRepositoryBase<TModel, TId>
    where TModel : class
{
    Task<IEnumerable<TModel>> GetAll();
    Task<TModel?> GetByIdOrDefault(TId id);
}