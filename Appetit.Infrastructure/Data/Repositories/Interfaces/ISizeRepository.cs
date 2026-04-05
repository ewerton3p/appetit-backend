using Appetit.Domain.Models;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories.Interfaces
{
    public interface ISizeRepository
    {
        Task<List<Size>> Get(Expression<Func<Size, bool>>? filter = null, Expression<Func<Size, object>>? order = null);
        Task<PaginatedResult<Size>> GetWithPagination(int page, Expression<Func<Size, bool>>? filter = null, Expression<Func<Size, object>>? order = null);
        Task<Size?> GetByIdAsync(int id);
        Task AddAsync(Size entity);
        Task DeleteAsync(Size entity);
        Task Update(Size entity);
    }
}
