using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Appetit.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepositoryBase<User> _repositoryBase;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(IRepositoryBase<User> repositoryBase, ApplicationDbContext dbContext)
        {
            _repositoryBase = repositoryBase;
            _dbContext = dbContext;
        }

        public async Task AddAsync(User entity)
        {
            await _repositoryBase.AddAsync(entity);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _repositoryBase.GetByIdAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
