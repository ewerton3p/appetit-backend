namespace Appetit.Application.DTOs.User
{
    public class UserViewDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
