using BlogAPI.Data;
using BlogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using BlogAPI.Middleware;
using AutoMapper;
using BlogAPI.Models.DTOs.Requests;
using BlogAPI.Models.DTOs.Responses;
using BlogAPI.Models.DTOs;

namespace BlogAPI.Services
{
    public class UserService(BlogDbContext dbContext, JwtOptions jwtOptions, IMapper mapper)
    {
        public async Task<RegisterResponseDtos> Register(RegisterRequestDtos request)
        {
            var existingUser = await dbContext.Set<User>().AnyAsync(x => x.Username == request.Username);
            if (existingUser)
                throw new ConflictException("Username already taken.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    Password = passwordHash,
                    Role = "Reader"
                };

            dbContext.Add(newUser);
            await dbContext.SaveChangesAsync();
            return mapper.Map<RegisterResponseDtos>(newUser);
        }

        public async Task<string?> Login(LoginDtos request)
        {
            var user = await dbContext.Set<User>().FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                throw new UnauthorizedException("Invalid username or password.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Signingkey)), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.Username),
                    new(ClaimTypes.Role, user.Role)
                })
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return accessToken;
        }

        public async Task<RegisterResponseDtos> GetUserById(int id)
        {
            var user = await dbContext.Set<User>().FindAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");
            return mapper.Map<RegisterResponseDtos>(user);
        }

        public async Task<string> UpgradeToAuthor(int id)
        {
            var user = await dbContext.Set<User>().FindAsync(id);
            if (user == null)
                throw new NotFoundException("User not found.");

            user.Role = "Author";
            await dbContext.SaveChangesAsync();
            return ($"{user.Username} is now an Author.");
        }
    }
}
