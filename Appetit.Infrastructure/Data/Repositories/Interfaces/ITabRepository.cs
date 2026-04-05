using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories.Interfaces
{
    public interface ITabRepository
    {
        Task<List<Tab>> Get(Expression<Func<Tab, bool>>? filter = null, Expression<Func<Tab, object>>? order = null);
        Task<PaginatedResult<Tab>> GetWithPagination(int page, Expression<Func<Tab, bool>>? filter = null, Expression<Func<Tab, object>>? order = null);
        Task<Tab?> GetByIdAsync(int id);
        Task AddAsync(Tab entity);
        Task DeleteAsync(Tab entity);
        Task Update(Tab entity);
    }
}
