using System.ComponentModel.DataAnnotations;

namespace Appetit.Application.DTOs.User
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        [MaxLength(255, ErrorMessage = "O Nome deve ter no máximo 255 caracteres.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [MaxLength(255, ErrorMessage = "O E-mail deve ter no máximo 255 caracteres.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A Senha deve ter no mínimo 6 caracteres.")]
        public required string Password { get; set; }
    }
}
