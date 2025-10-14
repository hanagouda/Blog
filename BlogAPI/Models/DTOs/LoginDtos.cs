using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTOs
{
    public class LoginDtos
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
