using AutoMapper;
using BlogAPI.Data;
using BlogAPI.Mapping;
using BlogAPI.Middleware;
using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using BlogAPI.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BlogUnitTests
{
    public class PostServiceTests
    {
        public static BlogDbContext NewInMemory(string name) =>
            new(new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName : name)
            .Options);
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task GetAllPosts_Paginates_And_Sorts()
        {
            await using var db = NewInMemory(nameof(GetAllPosts_Paginates_And_Sorts));

            var user = new User { Id = 1, Username = "TestUser", Email = "test@example.com", Password = "123", Role = "Author"};
            var category = new Category { Id = 1, Name = "Tech" };
            db.AddRange(user, category);

            db.AddRange(
                new Post { Title = "B", Content = "...", Status = "Published", CategoryId = 1, AuthorId = 1, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Post { Title = "A", Content = "...", Status = "Published", CategoryId = 1, AuthorId = 1, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new Post { Title = "C", Content = "...", Status = "Draft", CategoryId = 1, AuthorId = 1, CreatedAt = DateTime.UtcNow }
                );
            await db.SaveChangesAsync();

            var mapper = CreateMapper();
            var svc = new PostService(db, mapper);

            // Act
            var result = await svc.GetAllPosts(
                page: 1,
                pageSize: 1,
                search: null,
                categoryId: null,
                status: "Published",
                sort: "title_asc"
            );

            // Assert
            result.Total.Should().Be(2);
            result.Items.Should().HaveCount(1);           
            result.Items.First().Title.Should().Be("A");
        }

        [Fact]
        public async Task DeletePost_Throws_Conflict_When_User_Is_Not_Author_Or_Admin()
        {
            await using var db = NewInMemory(nameof(DeletePost_Throws_Conflict_When_User_Is_Not_Author_Or_Admin));
            db.Add(new Post { Id = 1, Title = "A", Content = "...", Status = "Published", CategoryId = 1, AuthorId = 1, CreatedAt = DateTime.UtcNow.AddDays(-1) });
            await db.SaveChangesAsync();

            var mapper = CreateMapper();
            var svc = new PostService(db, mapper);

            var act = async () => await svc.DeletePost(id: 1, userId: 999, userRole: "Reader");
            await act.Should().ThrowAsync<ForbiddenException>()
                .WithMessage("You are not allowed to delete this post.");
        }

    }
}
