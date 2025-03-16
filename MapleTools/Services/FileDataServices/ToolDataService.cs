using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models.Content;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MapleTools.Services.FileDataServices
{
    public class ToolDataService : FileDataService<ConcurrentDictionary<string, List<Tool>>>
    {

        public ToolDataService(IOptions<ServiceOptions> serviceOptions,IFileAccessor fileAccessor, IWebHostEnvironment webHostEnvironment, IOptions<LocalizationOptions> options) : base(fileAccessor,options)
        {
            Data = new ConcurrentDictionary<string, List<Tool>>();
            FilePath = new Dictionary<string, string>();
            foreach (var language in Languages)
            {
                FilePath.Add(language, Path.Combine(webHostEnvironment.ContentRootPath, serviceOptions.Value?.ToolDataService ?? "Data\\Tools", $"{language}.json"));
            }
        }

        public async override Task Aggregate()
        {
            if (Data.Count > 0)
                return;
            foreach (var path in FilePath)
            {
                var result = await FileAccessor.JsonFileReader<List<Tool>>(path.Value);
                Data.TryAdd(path.Key, result);
            }
            await base.Aggregate();
        }
    }
}
