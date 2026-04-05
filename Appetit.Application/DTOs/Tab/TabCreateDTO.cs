using Appetit.Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Appetit.Application.DTOs.Tab
{
    public class TabCreateDTO
    {
        [Required(ErrorMessage = "O Identificador é obrigatório.")]
        [MaxLength(255, ErrorMessage = "O Identificador deve ter no máximo 255 caracteres.")]
        public required string Identifier { get; set; }

        [Required(ErrorMessage = "O campo Ativo é obrigatório.")]
        public required bool Active { get; set; }

        [Required(ErrorMessage = "O Tipo é obrigatório.")]
        public required TabType Type { get; set; }
    }
}
