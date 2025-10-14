namespace BlogAPI.Models.DTOs.Responses
{
    public class PostResponseDtos
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ViewCount { get; set; }

        public string? AuthorUsername { get; set; }
        public string? CategoryName { get; set; }
        public int TagsCount { get; set; }
        public int LikesCount { get; set; }

        public List<CommentResponseDtos> Comments { get; set; }
    }
}
