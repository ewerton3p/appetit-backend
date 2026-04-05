using System.ComponentModel.DataAnnotations;

namespace Appetit.Application.DTOs.User
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        public required string Password { get; set; }
    }
}
