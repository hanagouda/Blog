using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController(PostService postService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResultDtos<PostResponseDtos>>> GetAllPosts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? status = null,
            [FromQuery] string sort = "createdAt_desc"
            )
        {
            var posts = await postService.GetAllPosts(page, pageSize, search, categoryId, status, sort);
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostResponseDtos>> GetPostById(int id)
        {
            var post = await postService.GetPostById(id);
            return Ok(post);
        }

        [HttpPost]
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<PostResponseDtos>> CreatePost([FromBody] PostRequestDtos request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var newPost = await postService.CreatePost(request, userId);
            return Ok(newPost);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<PostResponseDtos>> UpdatePost(int id, PostRequestDtos request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var updatedPost = await postService.UpdatePost(id, request, userId, userRole);
            return Ok(updatedPost);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<PostResponseDtos>> DeletePost(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var deletedPost = await postService.DeletePost(id, userId, userRole);
            return Ok(deletedPost);
        }

        [HttpPost("{postId}/Tags/{tagId}")]
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<PostTag>> AddTagToPost(int postId, int tagId)
        {
            var postTag = await postService.AddTagToPost(postId, tagId);
            return Ok(postTag);
        }

        [HttpDelete("{postId}/Tags/{tagId}")]
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<PostTag>> RemoveTagFromPost(int postId, int tagId)
        {
            var postTag = await postService.RemoveTagFromPost(postId, tagId);
            return Ok(postTag);
        }
    }
}
