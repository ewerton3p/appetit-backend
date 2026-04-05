namespace Appetit.Application.DTOs.User
{
    public class LoginResponseDTO
    {
        public required string Token { get; set; }
        public required UserViewDTO User { get; set; }
    }
}
