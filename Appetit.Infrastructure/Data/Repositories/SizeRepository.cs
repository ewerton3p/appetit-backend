using Appetit.Domain.Common.Utils;
using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories
{
    public class SizeRepository : ISizeRepository
    {
        private readonly IRepositoryBase<Size> _repositoryBase;
        private readonly ApplicationDbContext _dbContext;

        public SizeRepository(IRepositoryBase<Size> repositoryBase, ApplicationDbContext dbContext)
        {
            _repositoryBase = repositoryBase;
            _dbContext = dbContext;
        }

        public async Task AddAsync(Size entity)
        {
            await _repositoryBase.AddAsync(entity);
        }

        public async Task DeleteAsync(Size entity)
        {
            await _repositoryBase.DeleteAsync(entity);
        }

        public async Task<List<Size>> Get(Expression<Func<Size, bool>>? filter = null, Expression<Func<Size, object>>? order = null)
        {
            return await _repositoryBase.Get(filter, order);
        }

        public async Task<Size?> GetByIdAsync(int id)
        {
            return await _dbContext.Sizes
                .Include(s => s.CreatedBy)
                .Include(s => s.UpdatedBy)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<PaginatedResult<Size>> GetWithPagination(int page, Expression<Func<Size, bool>>? filter = null, Expression<Func<Size, object>>? order = null)
        {
            return await _repositoryBase.GetWithPagination(page, filter, order);
        }

        public async Task Update(Size entity)
        {
            entity.UpdatedAt = DateUtils.GetCurrentUtcDateTime();
            await _repositoryBase.Update(entity);
        }
    }
}
