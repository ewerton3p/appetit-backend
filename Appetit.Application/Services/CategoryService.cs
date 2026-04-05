using Appetit.Application.DTOs.Category;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Exceptions;
using Appetit.Domain.Common.Responses;
using Appetit.Domain.Models;
using Appetit.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Appetit.Application.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Response> CreateCategory(CategoryCreateDTO newCategory)
        {
            Response response = new();

            Category category = newCategory.ToCategory();
            
            await _categoryRepository.AddAsync(category);

            response.Message = "Categoria criada com sucesso.";

            return response;
            
        }

        public async Task<Response> DeleteCategory(int id)
        {
            Response response = new();

            try
            {
                Category? category = await _categoryRepository.GetByIdAsync(id) ?? throw new NotFoundException("Categoria não localizada.");

                await _categoryRepository.DeleteAsync(category);

                response.Message = "Categoria excluída com sucesso.";

            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("REFERENCE", StringComparison.OrdinalIgnoreCase) == true)
                {
                    response.Message = "Não é possível excluir esta categoria porque ela está associada a um ou mais cadastros.";
                    response.Success = false;

                }
                else
                {
                    response.Message = "Ocorreu um erro ao tentar excluir a categoria.";
                    response.Success = false;
                }
            }

            return response;
        }

        public async Task<ResponseData<List<CategoryViewDTO>>> GetCategories(int page, string search)
        {
            
            ResponseData<List<CategoryViewDTO>> response = new();

            Expression<Func<Category, bool>> filter = c => string.IsNullOrEmpty(search) || c.Name.Contains(search);
            Expression<Func<Category, object>> order = c => c.Name;

            var categories = await _categoryRepository.GetWithPagination(page, filter, order);
            response.Data = categories.Items?.Select(c => c.ToCategoryViewDTO()).ToList();
            response.Pagination =new Pagination(page, categories.TotalCount);

            if (response.Data == null || response.Data.Count == 0)
            {
                response.Message = "Nenhuma categoria encontrada.";
            }
            
            return response;

        }

        public async Task<ResponseData<CategoryViewDTO>> GetCategoryById(int id)
        {
            ResponseData<CategoryViewDTO> response = new();

            Category? category = await _categoryRepository.GetByIdAsync(id) ?? throw new NotFoundException($"Categoria com o código: {id} não foi localizada.");

            response.Data = category.ToCategoryViewDTO();

            return response;
        }

        public async Task<Response> UpdateCategory(int id, CategoryCreateDTO updatedCategory)
        {
            Response response = new();

            Category? category = _categoryRepository.GetByIdAsync(id).Result ?? throw new NotFoundException($"Categoria com o código: {id} não foi localizada.");

            category = updatedCategory.ToCategory();
            
            await _categoryRepository.Update(category);

            response.Message = "Categoria atualizada com sucesso.";

            return response;
        }
    }
}
