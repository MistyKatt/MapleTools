using Newtonsoft.Json;
using MapleTools.Models.Boss;

namespace MapleTools.Models.Boss
{
    public class Boss:IdBasedModel
    {
        [JsonProperty("boss name")]
        public required string Name { get; set; }

        [JsonProperty("boss img")]
        public string? ImageUrl { get; set; }

        [JsonProperty("boss details")]
        public string? DetailsUrl { get; set; }

        [JsonProperty("boss difficulty")]
        public Dictionary<string, BossDifficulty> Difficulties { get; set; }
    }
}
