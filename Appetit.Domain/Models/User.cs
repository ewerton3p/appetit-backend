using Appetit.Domain.Common.Utils;
using System.ComponentModel.DataAnnotations;

namespace Appetit.Domain.Models
{
    public class User : Entity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(255)]
        public required string Email { get; set; }

        [MaxLength(255)]
        public required string PasswordHash { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateUtils.GetCurrentUtcDateTime();

        public DateTimeOffset UpdatedAt { get; set; } = DateUtils.GetCurrentUtcDateTime();
    }
}
