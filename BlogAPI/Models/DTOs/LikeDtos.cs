using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models.DTOs
{
    public class LikeDtos
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
        [Range(1, int.MaxValue)]
        public int PostId { get; set; }
    }
}
