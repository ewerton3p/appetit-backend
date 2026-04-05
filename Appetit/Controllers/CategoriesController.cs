using Microsoft.AspNetCore.Authorization;
using Appetit.Application.DTOs.Category;
using Appetit.Application.Interfaces;
using Appetit.Domain.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Appetit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<CategoryViewDTO>>>> GetCategories(int page = 1, string search = "")
        {
            return Ok(await _categoryService.GetCategories(page, search));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<CategoryViewDTO>>> GetCategoryById(int id)
        {
            return Ok(await _categoryService.GetCategoryById(id));
        }

        [HttpPost("Category")]
        public async Task<ActionResult<Response>> CreateCategory(CategoryCreateDTO newCategory)
        {
            return Created(nameof(CreateCategory), await _categoryService.CreateCategory(newCategory));
        }

        [HttpPut("Category/{id}")]
        public async Task<ActionResult<Response>> UpdateCategory(CategoryCreateDTO updatedCategory, int id)
        {
            return Ok(await _categoryService.UpdateCategory(id, updatedCategory));
        }

        [HttpDelete("Category/{id}")]
        public async Task<ActionResult<Response>> DeleteCategory(int id)
        {
            return Ok(await _categoryService.DeleteCategory(id));
        }


    }
}
