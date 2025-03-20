using Newtonsoft.Json;

namespace MapleTools.Models.Content
{
    public class Blog
    {
        [JsonProperty("id")]
        public required string Id { get; set; }
        [JsonProperty("blog title")]
        public required string Title { get; set; }
        [JsonProperty("tool description")]
        public required string HtmlContent { get; set; }
        [JsonProperty("external link")]
        public string? ExternalLink { get; set; }
        [JsonProperty("internal link")]
        public string? InternalLink { get; set; }
        [JsonProperty("is external")]
        public required bool IsExternal { get; set; }

        [JsonProperty("game stage")]
        public string Stage {  get; set; }
    }
}
