using Newtonsoft.Json;
using MapleTools.Models.Boss;

namespace MapleTools.Models.Boss
{
    public class BossDifficulty
    {
        [JsonProperty("boss money")]
        public string Money { get; set; }

        [JsonProperty("boss arc")]
        public string ArcaneSymbol { get; set; }

        [JsonProperty("boss sac")]
        public string SacredSymbol { get; set; }

        [JsonProperty("boss HP")]
        public BossHP Hp { get; set; }

        [JsonProperty("blue light HP")]
        public string BlueLightHp { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("defense")]
        public string Defense { get; set; }

        [JsonProperty("loot")]
        public List<string> Loot { get; set; }
    }
}
