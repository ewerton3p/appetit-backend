using Appetit.Domain.Common.Utils;
using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Appetit.Infrastructure.Data.Repositories
{
    public class CategoryRepository:ICategoryRepository
    {

        private readonly IRepositoryBase<Category> _repositoryBase;
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(IRepositoryBase<Category> repositoryBase, ApplicationDbContext dbContext)
        {
            _repositoryBase = repositoryBase;
            _dbContext = dbContext;
        }

        public async Task AddAsync(Category entity)
        {
            await _repositoryBase.AddAsync(entity);
        }

        public async Task DeleteAsync(Category entity)
        {
            await _repositoryBase.DeleteAsync(entity);
        }

        public async Task<List<Category>> Get(Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, object>>? order = null)
        {
            return await _repositoryBase.Get(filter, order);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _repositoryBase.GetByIdAsync(id);
        }

        public async Task<PaginatedResult<Category>> GetWithPagination(int page, Expression<Func<Category, bool>>? filter = null, Expression<Func<Category, object>>? order = null)
        {
            return await _repositoryBase.GetWithPagination(page, filter, order);
        }

        public async Task Update(Category entity)
        {
            entity.UpdatedAt = DateUtils.GetCurrentUtcDateTime();
            await _repositoryBase.Update(entity);
        }
    }
}
