using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTOs.Requests
{
    public class CommentRequestDtos
    {
        [Required, MinLength(10)]
        public string Content { get; set; }
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
        [Range(1, int.MaxValue)]
        public int PostId { get; set; }
    }
}
