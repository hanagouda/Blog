using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController(CommentService commentService) : ControllerBase
    {
        [HttpGet("{postId}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDtos>>> GetAllComments(int postId)
        {
            var comments = await commentService.GetAllComments(postId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<ActionResult<CommentResponseDtos>> CreateComment([FromBody] CommentRequestDtos request)
        {
            var newComment = await commentService.CreateComment(request);
            return Ok(newComment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CommentResponseDtos>> DeleteComment(int id)
        {
            var deletedComment = await commentService.DeleteComment(id);
            return Ok(deletedComment);
        }
    }
}
