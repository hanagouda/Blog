using BlogAPI.Data;
using BlogAPI.Middleware;
using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services
{
    public class CategoryService(BlogDbContext dbContext)
    {
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await dbContext.Set<Category>().ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            var category = await dbContext.Set<Category>().FindAsync(id);
            if (category == null)
                throw new NotFoundException("Category not found.");
            return category;
        }

        public async Task<Category> CreateCategory(CategoryDtos request)
        {
            var newCategory = new Category
            {
                Name = request.Name
            };

            dbContext.Add(newCategory);
            await dbContext.SaveChangesAsync();
            return newCategory;
        }
    }
}
