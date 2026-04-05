using Appetit.Domain.Models;

namespace Appetit.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
