using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(CategoryService categoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CategoryDtos request)
        {
            var newCategory = await categoryService.CreateCategory(request);
            return Ok(newCategory);
        }
    }
}
