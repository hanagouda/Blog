using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<PostTag>().ToTable("PostTags").HasKey(pt => new { pt.PostId, pt.TagId });
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Like>().ToTable("Likes");
        }
    }
}
