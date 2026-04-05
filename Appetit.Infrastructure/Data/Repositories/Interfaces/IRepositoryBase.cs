using Appetit.Domain;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity:Entity
    {
        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? order = null);
        Task<List<TEntity>> Get(IQueryable<TEntity> query, Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? order = null);
        int Count(Expression<Func<TEntity, bool>>? filter = null);
        Task<PaginatedResult<TEntity>> GetWithPagination(int page, Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? order = null, bool descending = false);
        Task<PaginatedResult<TEntity>> GetWithPagination(IQueryable<TEntity> query, int page, Expression<Func<TEntity, bool>>? filter = null, Expression<Func<TEntity, object>>? order = null, bool descending = false);
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task Update(TEntity entity);
    }
}
