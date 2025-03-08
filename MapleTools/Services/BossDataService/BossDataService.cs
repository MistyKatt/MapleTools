using MapleTools.Models;
using MapleTools.Models.Boss;
using Newtonsoft.Json;

namespace MapleTools.Services.BossDataService
{
    public class BossDataService
    {
        private List<Boss> _bosses;

        public BossDataService()
        {
            _bosses = new List<Boss>();
        }

        public List<Boss> Bosses { get { return _bosses; } }

        public async Task GetBossData(string filePath)
        {
            if (Bosses.Count > 0)
                return;
            if (File.Exists(filePath) && filePath.EndsWith("json"))
            {
                using (StreamReader rs = new StreamReader(filePath))
                {
                    var result = await rs.ReadToEndAsync();
                    _bosses = JsonConvert.DeserializeObject<List<Boss>>(result) ?? new List<Boss>();
                }
            }
        }
    }
}
