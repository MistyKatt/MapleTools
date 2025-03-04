using Newtonsoft.Json;

namespace MapleTools.Models
{
    public class Player
    {
        [JsonProperty("characterID")]
        public int CharacterID { get; set; }

        [JsonProperty("characterName")]
        public string CharacterName { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("jobID")]
        public int JobID { get; set; }

        //The exp value in current level
        [JsonProperty("exp")]
        public long Exp { get; set; }
        //The certain amount of exp gained in a period of time
        [JsonProperty("Gap")]
        public long Gap { get; set; }

        [JsonProperty("worldID")]
        public int WorldID { get; set; }

        [JsonProperty("worldName")]
        public string WorldName { get; set; }

        [JsonProperty("characterImgURL")]
        public string CharacterImgURL { get; set; }

        // Add other properties as needed
    }
}
