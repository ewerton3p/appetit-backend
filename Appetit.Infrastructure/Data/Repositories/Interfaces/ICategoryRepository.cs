using Appetit.Domain.Models;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> Get(Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, object>>? order = null);
        Task<PaginatedResult<Category>> GetWithPagination(int page, Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, object>>? order = null);
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category entity);
        Task DeleteAsync(Category entity);
        Task Update(Category entity);
    }
}
