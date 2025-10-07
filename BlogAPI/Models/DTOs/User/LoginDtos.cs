using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTOs.User
{
    public class LoginDtos
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
