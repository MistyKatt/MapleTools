using Newtonsoft.Json;

namespace MapleTools.Models.Content
{
    
    public class Tool
    {
        [JsonProperty("id")]
        public required string Id { get; set; }
        
        [JsonProperty("tool name")]
        public required string Name { get; set; }
        [JsonProperty("tool description")]
        public required string Description { get; set; }
        [JsonProperty("external link")]
        public string? ExternalLink { get; set; }
        [JsonProperty("internal link")]
        public string? InternalLink {  get; set; }
        [JsonProperty("is external")]
        public required bool IsExternal { get; set; }
    }
}
