using System.ComponentModel.DataAnnotations;

namespace Appetit.Application.DTOs.Category
{
    public class CategoryCreateDTO
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public required string Name { get; set; }
    }
}
