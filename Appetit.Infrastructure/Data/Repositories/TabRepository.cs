using Appetit.Domain.Common.Utils;
using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories
{
    public class TabRepository : ITabRepository
    {
        private readonly IRepositoryBase<Tab> _repositoryBase;
        private readonly ApplicationDbContext _dbContext;

        public TabRepository(IRepositoryBase<Tab> repositoryBase, ApplicationDbContext dbContext)
        {
            _repositoryBase = repositoryBase;
            _dbContext = dbContext;
        }

        public async Task AddAsync(Tab entity)
        {
            await _repositoryBase.AddAsync(entity);
        }

        public async Task DeleteAsync(Tab entity)
        {
            await _repositoryBase.DeleteAsync(entity);
        }

        public async Task<List<Tab>> Get(Expression<Func<Tab, bool>>? filter = null, Expression<Func<Tab, object>>? order = null)
        {
            return await _repositoryBase.Get(filter, order);
        }

        public async Task<Tab?> GetByIdAsync(int id)
        {
            return await _dbContext.Tabs
                .Include(t => t.CreatedBy)
                .Include(t => t.UpdatedBy)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<PaginatedResult<Tab>> GetWithPagination(int page, Expression<Func<Tab, bool>>? filter = null, Expression<Func<Tab, object>>? order = null)
        {
            return await _repositoryBase.GetWithPagination(page, filter, order);
        }

        public async Task Update(Tab entity)
        {
            entity.UpdatedAt = DateUtils.GetCurrentUtcDateTime();
            await _repositoryBase.Update(entity);
        }
    }
}
