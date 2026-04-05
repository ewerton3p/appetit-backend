using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IRepositoryBase<City> _repositoryBase;
        private readonly ApplicationDbContext _dbContext;

        public CityRepository(IRepositoryBase<City> repositoryBase, ApplicationDbContext dbContext)
        {
            _repositoryBase = repositoryBase;
            _dbContext = dbContext;
        }

        public async Task<City?> GetByIdAsync(int id)
        {
            return await _dbContext.Cities.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<PaginatedResult<City>> GetWithPagination(int page, Expression<Func<City, bool>>? filter = null, Expression<Func<City, object>>? order = null)
        {
            return await _repositoryBase.GetWithPagination(page, filter, order);
        }
    }
}
