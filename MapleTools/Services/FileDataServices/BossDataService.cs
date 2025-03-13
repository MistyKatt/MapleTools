using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models;
using MapleTools.Models.Boss;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MapleTools.Services.FileDataServices
{
    public class BossDataService:FileDataService
    {
        private List<Boss> _bosses;

        private string _filePath;

        public BossDataService(IOptions<ServiceOptions> serviceOptions, IWebHostEnvironment webHostEnvironment, IOptions<LocalizationOptions> options):base()
        {
            _bosses = new List<Boss>();
            _filePath = Path.Combine(webHostEnvironment.ContentRootPath, serviceOptions.Value?.BossDataService ?? "dummy");
        }

        public List<Boss> Bosses { get { return _bosses; } }

        public async override Task Aggregate()
        {
            if (Bosses.Count > 0)
                return;
            if (File.Exists(_filePath) && _filePath.EndsWith("json"))
            {
                using (StreamReader rs = new StreamReader(_filePath))
                {
                    var result = await rs.ReadToEndAsync();
                    _bosses = JsonConvert.DeserializeObject<List<Boss>>(result) ?? new List<Boss>();
                }
            }
            await base.Aggregate();
        }
    }
}
