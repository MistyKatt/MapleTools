namespace MapleTools.Models
{
    public class Article
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public ArticleType Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public enum ArticleType
    {
        News,
        Personal
    }
}
