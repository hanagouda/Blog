using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTOs.Requests
{
    public class PostRequestDtos
    {
        [Required, StringLength(150)]
        public string Title { get; set; }
        [Required, MinLength(10)]
        public string Content { get; set; }
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [RegularExpression("^(Draft|Published|Scheduled)$", ErrorMessage = "Status must be Draft, Published, or Scheduled.")]
        public string? Status { get; set; }
        public DateTime? ScheduledAt { get; set; }
    }
}
