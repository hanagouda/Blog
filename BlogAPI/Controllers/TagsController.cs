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
    [Authorize]
    public class TagsController(TagService tagService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllTags()
        {
            var categories = await tagService.GetAllTags();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTagById(int id)
        {
            var tag = await tagService.GetTagById(id);
            return Ok(tag);
        }

        [HttpPost]
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<Category>> CreateTag([FromBody] TagDtos request)
        {
            var newTag = await tagService.CreateTag(request);
            return Ok(newTag);
        }
    }
}
