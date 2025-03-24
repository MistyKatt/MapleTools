using MapleTools.Abstraction;

namespace MapleTools.Models.Boss
{
    public class BossArticle:IdBasedModel, IArticle
    {
        public string ContentPath{ get; set; }

        public string HtmlContent { get; set; }
    }
}
