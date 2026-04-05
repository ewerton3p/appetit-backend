namespace Appetit.Application.DTOs.Size
{
    public static class SizeExtension
    {
        public static SizeViewDTO ToSizeViewDTO(this Domain.Models.Size size) => new()
        {
            Id = size.Id,
            Description = size.Description,
            Abbreviation = size.Abbreviation,
            CreatedById = size.CreatedById,
            CreatedByName = size.CreatedBy?.Name ?? "",
            UpdatedById = size.UpdatedById,
            UpdatedByName = size.UpdatedBy?.Name ?? "",
            CreatedAt = size.CreatedAt,
            UpdatedAt = size.UpdatedAt
        };

        public static Domain.Models.Size ToSize(this SizeCreateDTO dto) => new()
        {
            Description = dto.Description,
            Abbreviation = dto.Abbreviation
        };

        public static Domain.Models.Size ToSize(this SizeCreateDTO dto, Domain.Models.Size size)
        {
            size.Description = dto.Description;
            size.Abbreviation = dto.Abbreviation;
            return size;
        }
    }
}
