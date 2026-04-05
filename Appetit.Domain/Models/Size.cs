using Appetit.Domain.Common.Utils;
using System.ComponentModel.DataAnnotations;

namespace Appetit.Domain.Models
{
    public class Size : Entity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public required string Description { get; set; }

        [MaxLength(10)]
        public required string Abbreviation { get; set; }

        public User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }

        public User? UpdatedBy { get; set; }
        public int? UpdatedById { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateUtils.GetCurrentUtcDateTime();
        public DateTimeOffset UpdatedAt { get; set; } = DateUtils.GetCurrentUtcDateTime();
    }
}
