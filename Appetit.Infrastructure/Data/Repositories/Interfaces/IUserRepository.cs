using Appetit.Domain.Models;

namespace Appetit.Infrastructure.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User entity);
    }
}
