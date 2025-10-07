using AutoMapper;
using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace BlogAPI.Services
{
    public class CommentService(BlogDbContext dbContext, IMapper mapper)
    {
        public async Task<IEnumerable<CommentResponseDtos>> GetAllComments(int postId)
        {
            return await dbContext.Set<Comment>().AsNoTracking().Where(c => c.PostId == postId)
                .Include(d => d.User)
                .ProjectTo<CommentResponseDtos>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<CommentResponseDtos> CreateComment([FromBody] CommentRequestDtos request)
        {
            var newComment = new Comment
            {
                Content = request.Content,
                UserId = request.UserId,
                PostId = request.PostId
            };

            dbContext.Add(newComment);
            await dbContext.SaveChangesAsync();
            return mapper.Map<CommentResponseDtos>(newComment);
        }

        public async Task<CommentResponseDtos> DeleteComment(int id)
        {
            var deletedComment = await dbContext.Set<Comment>().FindAsync(id);
            if (deletedComment == null)
                return null;

            dbContext.Remove(deletedComment);
            await dbContext.SaveChangesAsync();
            return mapper.Map<CommentResponseDtos>(deletedComment);
        }
    }
}
