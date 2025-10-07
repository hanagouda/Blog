using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikesController(LikeService likeService) : ControllerBase
    {
        [HttpPost("{postId}")]
        public async Task<ActionResult<Like>> AddLikeToPost(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var like = await likeService.AddLikeToPost(userId, postId);
            return Ok(like);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Like>> RemoveLikeFromPost(int id)
        {
            var like = await likeService.RemoveLikeFromPost(id);
            return Ok(like);
        }
    }
}
