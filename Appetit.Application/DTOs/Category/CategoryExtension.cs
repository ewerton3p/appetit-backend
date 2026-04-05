namespace Appetit.Application.DTOs.Category
{
    public static class CategoryExtension
    {
        public static CategoryViewDTO ToCategoryViewDTO(this Domain.Models.Category category)
        {
            return new CategoryViewDTO
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

        public static List<CategoryViewDTO> ToCategoryViewDTOList(this List<Domain.Models.Category> categories)
        {
            return [.. categories.Select(c => c.ToCategoryViewDTO())];
        }

        public static Domain.Models.Category ToCategory(this CategoryCreateDTO categoryCreateDTO)
        {
            return new Domain.Models.Category
            {
                Name = categoryCreateDTO.Name
            };
        }
    }
}
