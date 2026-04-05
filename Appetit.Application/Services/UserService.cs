using Appetit.Application.DTOs.User;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Exceptions;
using Appetit.Domain.Common.Responses;
using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Appetit.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Appetit.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> Register(UserCreateDTO newUser)
        {
            Response response = new();

            var existingUser = await _userRepository.GetByEmailAsync(newUser.Email);

            if (existingUser != null)
            {
                response.Message = "Já existe um usuário cadastrado com este e-mail.";
                response.Success = false;
                return response;
            }

            var user = newUser.ToUser(string.Empty);
            user.PasswordHash = _passwordHasher.HashPassword(user, newUser.Password);

            await _userRepository.AddAsync(user);

            response.Message = "Usuário cadastrado com sucesso.";

            return response;
        }

        public Task<int> GetLoggedUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
                ?? throw new BadRequestException("Usuário não autenticado.");

            if (!int.TryParse(claim.Value, out var userId))
                throw new BadRequestException("Token inválido.");

            return Task.FromResult(userId);
        }

        public async Task<User> GetLoggedUser()
        {
            var userId = await GetLoggedUserId();

            return await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException("Usuário não localizado.");
        }

        public async Task<ResponseData<LoginResponseDTO>> Login(LoginDTO loginDTO)
        {
            ResponseData<LoginResponseDTO> response = new();

            var user = await _userRepository.GetByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                response.Message = "E-mail ou senha inválidos.";
                response.Success = false;
                return response;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                response.Message = "E-mail ou senha inválidos.";
                response.Success = false;
                return response;
            }

            var token = _tokenService.GenerateToken(user);

            response.Data = new LoginResponseDTO
            {
                Token = token,
                User = user.ToUserViewDTO()
            };
            response.Message = "Login realizado com sucesso.";
            response.Success = true;

            return response;
        }
    }
}
