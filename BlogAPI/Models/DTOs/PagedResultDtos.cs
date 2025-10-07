namespace BlogAPI.Models.DTOs
{
    public class PagedResultDtos<T>
    {
        public int Total {  get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}
