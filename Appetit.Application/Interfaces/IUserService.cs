using Appetit.Application.DTOs.User;
using Appetit.Domain.Common.Responses;
using Appetit.Domain.Models;

namespace Appetit.Application.Interfaces
{
    public interface IUserService
    {
        Task<Response> Register(UserCreateDTO newUser);
        Task<ResponseData<LoginResponseDTO>> Login(LoginDTO loginDTO);
        Task<User> GetLoggedUser();
        Task<int> GetLoggedUserId();
    }
}
