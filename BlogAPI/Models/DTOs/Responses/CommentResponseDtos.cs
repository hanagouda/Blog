namespace BlogAPI.Models.DTOs.Responses
{
    public class CommentResponseDtos
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserUsername { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
