namespace Appetit.Application.DTOs.User
{
    public static class UserExtension
    {
        public static UserViewDTO ToUserViewDTO(this Domain.Models.User user)
        {
            return new UserViewDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public static Domain.Models.User ToUser(this UserCreateDTO dto, string passwordHash)
        {
            return new Domain.Models.User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash
            };
        }
    }
}
