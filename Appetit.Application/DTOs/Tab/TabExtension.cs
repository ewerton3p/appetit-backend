namespace Appetit.Application.DTOs.Tab
{
    public static class TabExtension
    {
        public static TabViewDTO ToTabViewDTO(this Domain.Models.Tab tab) => new()
        {
            Id = tab.Id,
            Identifier = tab.Identifier,
            Active = tab.Active,
            Type = tab.Type,
            CreatedById = tab.CreatedById,
            CreatedByName = tab.CreatedBy?.Name ?? "",
            UpdatedById = tab.UpdatedById,
            UpdatedByName = tab.UpdatedBy?.Name ?? "",
            CreatedAt = tab.CreatedAt,
            UpdatedAt = tab.UpdatedAt
        };

        public static Domain.Models.Tab ToTab(this TabCreateDTO dto) => new()
        {
            Identifier = dto.Identifier,
            Active = dto.Active,
            Type = dto.Type
        };

        public static Domain.Models.Tab ToTab(this TabCreateDTO dto, Domain.Models.Tab tab)
        {
            tab.Identifier = dto.Identifier;
            tab.Active = dto.Active;
            tab.Type = dto.Type;
            return tab;
        }
    }
}
