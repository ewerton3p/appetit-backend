using Appetit.Application.DTOs.Category;
using Appetit.Domain.Common.Responses;

namespace Appetit.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ResponseData<List<CategoryViewDTO>>> GetCategories(int page, string search);
        Task<Response> CreateCategory(CategoryCreateDTO newCategory);
        Task<Response> UpdateCategory(int id, CategoryCreateDTO updatedCategory);
        Task<ResponseData<CategoryViewDTO>> GetCategoryById(int id);
        Task<Response> DeleteCategory(int id);
    }
}
