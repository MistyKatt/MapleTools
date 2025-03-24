using MapleTools.Abstraction;

namespace MapleTools.Models.Content
{
    public class BlogArticle:IdBasedModel, IArticle
    {
        public string HtmlContent { get; set; }

        public string ContentPath { get; set; }
    }
}
