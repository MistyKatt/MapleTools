using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using Newtonsoft.Json;

namespace MapleTools.Services.BossDataService
{
    public class ToolDataService
    {
        private List<Tool> _tools;

        public ToolDataService()
        {
            _tools = new List<Tool>();
        }

        public List<Tool> Tools { get { return _tools; } }

        public async Task GetToolData(string filePath)
        {
            if (Tools.Count > 0)
                return;
            if (File.Exists(filePath) && filePath.EndsWith("json"))
            {
                using (StreamReader rs = new StreamReader(filePath))
                {
                    var result = await rs.ReadToEndAsync();
                    _tools = JsonConvert.DeserializeObject<List<Tool>>(result) ?? new List<Tool>();
                }
            }
        }
    }
}
