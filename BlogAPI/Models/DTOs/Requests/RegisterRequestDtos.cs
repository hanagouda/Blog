using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTOs.Requests
{
    public class RegisterRequestDtos
    {
        [Required, MinLength(3)]
        public string Username { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
