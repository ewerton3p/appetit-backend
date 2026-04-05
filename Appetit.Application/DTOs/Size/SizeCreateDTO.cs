using System.ComponentModel.DataAnnotations;

namespace Appetit.Application.DTOs.Size
{
    public class SizeCreateDTO
    {
        [Required]
        [MaxLength(255)]
        public required string Description { get; set; }

        [Required]
        [MaxLength(10)]
        public required string Abbreviation { get; set; }
    }
}
