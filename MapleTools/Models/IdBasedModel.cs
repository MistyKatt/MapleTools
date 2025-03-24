using Newtonsoft.Json;

namespace MapleTools.Models
{
    public class IdBasedModel
    {
        [JsonProperty("id")]
        public required string Id { get; set; }
    }
}
