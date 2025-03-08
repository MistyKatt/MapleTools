using Newtonsoft.Json;

namespace MapleTools.Models.Boss
{
    public class BossHP
    {
        [JsonProperty("phase1")]
        public required string Phase1 { get; set; }

        [JsonProperty("phase2")]
        public string? Phase2 { get; set; }

        [JsonProperty("phase3")]
        public string? Phase3 { get; set; }

        [JsonProperty("phase4")]
        public string? Phase4 { get; set; }
    }
}
