using Appetit.Domain.Common.Enums;

namespace Appetit.Application.DTOs.Tab
{
    public class TabViewDTO
    {
        public int Id { get; set; }
        public required string Identifier { get; set; }
        public bool Active { get; set; }
        public TabType Type { get; set; }
        public int? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public int? UpdatedById { get; set; }
        public string? UpdatedByName { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
