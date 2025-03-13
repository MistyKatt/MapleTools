using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models.Content;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MapleTools.Services.FileDataServices
{
    public class ToolDataService : FileDataService
    {
        private ConcurrentDictionary<string, List<Tool>> _tools;

        private Dictionary<string,string> _filePath;

        public ToolDataService(IOptions<ServiceOptions> serviceOptions, IWebHostEnvironment webHostEnvironment, IOptions<LocalizationOptions> options) : base()
        {
            _tools = new ConcurrentDictionary<string, List<Tool>>();
            _filePath = new Dictionary<string, string>();
            foreach (var language in options.Value.Languages ?? ["en", "zh-CN"])
            {
                _filePath.Add(language, Path.Combine(webHostEnvironment.ContentRootPath, serviceOptions.Value?.ToolDataService ?? "Data\\Tools", $"{language}.json"));
            }
        }

        public ConcurrentDictionary<string, List<Tool>> Tools { get { return _tools; } }

        public async override Task Aggregate()
        {
            if (Tools.Count > 0)
                return;
            foreach (var path in _filePath)
            {
                if (File.Exists(path.Value) && path.Value.EndsWith("json"))
                {
                    using (StreamReader rs = new StreamReader(path.Value))
                    {
                        var result = await rs.ReadToEndAsync();
                        _tools.TryAdd(path.Key,JsonConvert.DeserializeObject<List<Tool>>(result) ?? new List<Tool>());
                    }
                }
            }
            await base.Aggregate();
        }
    }
}
