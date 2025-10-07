using AutoMapper;
using BlogAPI.Data;
using BlogAPI.Mapping;
using BlogAPI.Middleware;
using BlogAPI.Models;
using BlogAPI.Models.DTOs.User;
using BlogAPI.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BlogUnitTests
{
    public class UserServiceTests
    {
        public static BlogDbContext NewInMemoryDb(string name) =>
            new(new DbContextOptionsBuilder<BlogDbContext>()
                // Instead of connecting to a real SQL database, it makes a fake in-memory one just for tests.
                .UseInMemoryDatabase(databaseName: name)  // databaseName: name → Each test gets a unique in-memory DB name, so they don’t interfere with each other.
                .Options);
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task Register_Throws_Conflict_When_Username_Already_Exists()
        {
            await using var db = NewInMemoryDb(nameof(Register_Throws_Conflict_When_Username_Already_Exists));
            db.Add(new User { Username = "hana", Email = "a@b.com", Password = "hash", Role = "Reader" });
            await db.SaveChangesAsync();

            var jwt = new JwtOptions { Issuer = "x", Audience = "y", Signingkey = "secretsecretsecretsecret" };
            var mapper = CreateMapper();
            var svc = new UserService(db, jwt, mapper);

            var act = async () => await svc.Register(new RegisterRequestDtos
            { Username = "hana", Email = "z@b.com", Password = "password123" });

            await act.Should().ThrowAsync<ConflictException>()
                .WithMessage("*Username already taken*"); ;
        }

        [Fact]
        public async Task Login_Throws_Conflict_When_On_Wrong_Password()
        {
            await using var db = NewInMemoryDb(nameof(Login_Throws_Conflict_When_On_Wrong_Password));
            db.Add(new User { Username = "hana", Email = "a@b.com", Password = BCrypt.Net.BCrypt.HashPassword("correct"), Role = "Reader" });
            await db.SaveChangesAsync();

            var jwt = new JwtOptions { Issuer = "x", Audience = "y", Signingkey = "secretsecretsecretsecret" };
            var mapper = CreateMapper();
            var svc = new UserService(db, jwt, mapper);

            var act = async () => await svc.Login(new LoginDtos
            { Username = "hana", Password = "wrong" });

            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("*Invalid username or password*");
        }
    }
}
