using Domain.Specifications;

namespace Application.Common.Interfaces;

public interface IQuery<TEntity> where TEntity : Domain.Common.Entity
{
    Task<IReadOnlyList<TEntity>> Find(Spec<TEntity> spec, SortSpec<TEntity>? sortSpec, int pageNumber = 1,
        int pageSize = int.MaxValue);

    Task<IReadOnlyList<TEntity>> Find(Spec<TEntity> spec, SortSpec<TEntity>? sortSpec,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? includes);

    Task<IReadOnlyList<TEntity>> Find(Spec<TEntity> spec);
    Task<TEntity?> FindById(int id);
    Task<TEntity?> FindById(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>> includes);
}