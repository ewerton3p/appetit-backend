using System.ComponentModel.DataAnnotations;

namespace Appetit.Domain.Models
{
    public class City : Entity
    {
        [Key]
        public int Id { get; set; }

        public int Ibge { get; set; }

        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(2)]
        public required string Uf { get; set; }
    }
}
