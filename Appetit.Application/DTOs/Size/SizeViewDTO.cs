namespace Appetit.Application.DTOs.Size
{
    public class SizeViewDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public int? CreatedById { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public int? UpdatedById { get; set; }
        public string UpdatedByName { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
