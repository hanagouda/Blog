using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.DTOs;
using BlogAPI.Models.DTOs.Requests;
using BlogAPI.Models.DTOs.Responses;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserService userService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<RegisterResponseDtos>> Register(RegisterRequestDtos request)
        {
            var newUser = await userService.Register(request);
            return Ok(newUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(LoginDtos request)
        {
            var user = await userService.Login(request);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegisterResponseDtos>> GetUserById(int id)
        {
            var user = await userService.GetUserById(id);
            return Ok(user);
        }

        [HttpPatch("{id}")]  // PATCH → used for partial updates (e.g., only updating Email).
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> UpgradeToAuthor(int id)
        {
            var user = await userService.UpgradeToAuthor(id);
            return Ok(user);
        }
    }
}
