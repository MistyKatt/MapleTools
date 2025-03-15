using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models;
using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MapleTools.Services.FileDataServices
{
    public class BossDataService:FileDataService
    {
        private ConcurrentDictionary<string, List<Boss>> _bosses;

        private Dictionary<string, string> _filePath;

        public BossDataService(IOptions<ServiceOptions> serviceOptions, IWebHostEnvironment webHostEnvironment, IOptions<LocalizationOptions> options):base()
        {
            _bosses = new ConcurrentDictionary<string, List<Boss>>();
            _filePath = new Dictionary<string, string>();
            foreach (var language in options.Value.Languages ?? ["en", "zh-CN"])
            {
                _filePath.Add(language, Path.Combine(webHostEnvironment.ContentRootPath, serviceOptions.Value?.BossDataService ?? "Data\\Bosses", $"{language}.json"));
            }
        }

        public ConcurrentDictionary<string, List<Boss>> Bosses { get { return _bosses; } }

        public async override Task Aggregate()
        {
            if (Bosses.Count > 0)
                return;
            foreach (var path in _filePath)
            {
                if (File.Exists(path.Value) && path.Value.EndsWith("json"))
                {
                    using (StreamReader rs = new StreamReader(path.Value))
                    {
                        var result = await rs.ReadToEndAsync();
                        _bosses.TryAdd(path.Key, JsonConvert.DeserializeObject<List<Boss>>(result) ?? new List<Boss>());
                    }
                }
            }
            await base.Aggregate();
        }
    }
}
