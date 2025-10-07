using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services
{
    public class LikeService(BlogDbContext dbContext)
    {
        public async Task<string> AddLikeToPost(int userId, int postId)
        {
            var like = await dbContext.Set<Like>().AnyAsync(l => l.UserId == userId && l.PostId == postId);
            if (like)
                return ("Already liked.");

            var addedLike = new Like
            {
                UserId = userId,
                PostId = postId
            };

            dbContext.Add(addedLike);
            await dbContext.SaveChangesAsync();
            return ("Post liked.");
        }

        public async Task<string> RemoveLikeFromPost(int id)
        {
            var deletedlike = await dbContext.Set<Like>().FindAsync(id);
            if (deletedlike == null)
                return ("Not liked yet.");

            dbContext.Remove(deletedlike);
            await dbContext.SaveChangesAsync();
            return ("Post unliked.");
        }
    }
}
