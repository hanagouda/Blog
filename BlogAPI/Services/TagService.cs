using Azure;
using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Middleware;

namespace BlogAPI.Services
{
    public class TagService(BlogDbContext dbContext)
    {
        public async Task<IEnumerable<Tag>> GetAllTags()
        {
            return await dbContext.Set<Tag>().ToListAsync();
        }

        public async Task<Tag> GetTagById(int id)
        {
            var tag = await dbContext.Set<Tag>().FindAsync(id);
            if (tag == null)
                throw new NotFoundException("Tag not found.");
            return tag;
        }

        public async Task<Tag> CreateTag(TagDtos request)
        {
            var newTag = new Tag
            {
                Name = request.Name
            };

            dbContext.Add(newTag);
            await dbContext.SaveChangesAsync();
            return newTag;
        }
    }
}
