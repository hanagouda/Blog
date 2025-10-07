using AutoMapper;
using AutoMapper.QueryableExtensions;
using BlogAPI.Data;
using BlogAPI.Middleware;
using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Hosting;

namespace BlogAPI.Services
{
    public class PostService(BlogDbContext dbContext, IMapper mapper)
    {
        public async Task<PagedResultDtos<PostResponseDtos>> GetAllPosts(int page, int pageSize, string? search, int? categoryId, string? status, string sort)
        {
            var query = dbContext.Set<Post>().AsNoTracking().Where(p => p.Status == "Published" || (p.Status == "Scheduled" && p.ScheduledAt <= DateTime.Now));  // AsNoTracking() → less memory usage, faster queries.

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(status))  // Example: admin wants to see Draft.
                query = query.Where(p => p.Status == status);

            query = sort switch
            {
                "createdAt_asc" => query.OrderBy(p => p.CreatedAt),
                "title_asc" => query.OrderBy(p => p.Title),
                "title_desc" => query.OrderByDescending(p => p.Title),
                "views_desc" => query.OrderByDescending(p => p.ViewCount),
                _ => query.OrderByDescending(p => p.CreatedAt) // createdAt_desc default
            };

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                .Include(p => p.Comments).ThenInclude(p => p.User)  // load comment authors
                .Include(p => p.Likes)
                .ProjectTo<PostResponseDtos>(mapper.ConfigurationProvider)  // ProjectTo = tell the database to fetch only what the DTO needs, then return it directly. For list queries.
                .ToListAsync();

            return new PagedResultDtos<PostResponseDtos>
            { 
                Total = total, 
                Page = page, 
                PageSize = pageSize, 
                Items= items 
            };
        }

        public async Task<PostResponseDtos> GetPostById(int id)
        {
            var post = await dbContext.Set<Post>()
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Include(p => p.PostTags)
                .Include(p => p.Comments).ThenInclude(p => p.User)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
                throw new NotFoundException("Post not found.");

            post.ViewCount++;
            await dbContext.SaveChangesAsync();
            return mapper.Map<PostResponseDtos>(post);  // Map = take something you already loaded in memory and copy it into a DTO. For single-entity operations.
            // Map works after fetching, ProjectTo works while fetching.
        }

        public async Task<PostResponseDtos> CreatePost(PostRequestDtos request, int userId)
        {
            var newPost = new Post
            {
                Title = request.Title,
                Content = request.Content,
                AuthorId = userId,
                CategoryId = request.CategoryId,
                Status = string.IsNullOrEmpty(request.Status) ? "Draft" : request.Status,
                ScheduledAt = request.Status == "Scheduled" ? request.ScheduledAt : null
            };

            if (newPost.Status == "Scheduled" && newPost.ScheduledAt == null)
                throw new BadRequestException("Scheduled posts must have a ScheduledAt date.");

            dbContext.Add(newPost);
            await dbContext.SaveChangesAsync();
            return mapper.Map<PostResponseDtos>(newPost);
        }

        public async Task<PostResponseDtos> UpdatePost(int id, PostRequestDtos updatedPost, int userId, string userRole)
        {
            var existingPost = await dbContext.Set<Post>().FindAsync(id);
            if (existingPost == null)
                throw new NotFoundException("Post not found.");

            if (userRole != "Admin" && userId != existingPost.AuthorId)
                throw new ForbiddenException("You are not allowed to edit this post.");

            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.CategoryId = updatedPost.CategoryId;
            existingPost.UpdatedAt = DateTime.Now;

            await dbContext.SaveChangesAsync();
            return mapper.Map<PostResponseDtos>(updatedPost);
        }

        public async Task<PostResponseDtos> DeletePost(int id, int userId, string userRole)
        {
            var deletedPost = await dbContext.Set<Post>().FindAsync(id);
            if (deletedPost == null)
                throw new NotFoundException("Post not found.");

            // Admins can delete any post, Authors only their own.
            if (userRole != "Admin" && userId != deletedPost.AuthorId)
                throw new ForbiddenException("You are not allowed to delete this post.");

            dbContext.Remove(deletedPost);
            await dbContext.SaveChangesAsync();
            return mapper.Map<PostResponseDtos>(deletedPost);
        }

        public async Task<string> AddTagToPost(int postId, int tagId)
        {
            var postTag = await dbContext.Set<PostTag>().AnyAsync(pt => pt.PostId == postId && pt.TagId == tagId);  // Any() → when you only care if something exists (yes/no).
            if (postTag)  // if exists
                throw new ConflictException("Tag already added to this post.");

            var addedPostTag = new PostTag
            {
                PostId = postId,
                TagId = tagId
            };

            dbContext.Add(addedPostTag);
            await dbContext.SaveChangesAsync();
            return ("Tag added to post.");
        }

        public async Task<string> RemoveTagFromPost(int postId, int tagId)
        {
            var deletedPostTag = await dbContext.Set<PostTag>().FirstOrDefaultAsync(pt => pt.PostId == postId && pt.TagId == tagId);  // FirstOrDefault() → when you need to actually get the record.
            if (deletedPostTag == null)
                throw new NotFoundException("Tag link not found.");

            dbContext.Remove(deletedPostTag);
            await dbContext.SaveChangesAsync();
            return ("Tag removed from post.");
        }
    }
}
