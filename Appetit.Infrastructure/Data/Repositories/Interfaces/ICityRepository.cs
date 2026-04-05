using Appetit.Domain.Models;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories.Interfaces
{
    public interface ICityRepository
    {
        Task<PaginatedResult<City>> GetWithPagination(int page, Expression<Func<City, bool>>? filter = null, Expression<Func<City, object>>? order = null);
        Task<City?> GetByIdAsync(int id);
    }
}
