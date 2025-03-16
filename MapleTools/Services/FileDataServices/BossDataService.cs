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
    public class BossDataService:FileDataService<ConcurrentDictionary<string, List<Boss>>>
    {

        public BossDataService(IOptions<LocalizationOptions> cultureOptions,IFileAccessor fileAccessor,IOptions<ServiceOptions> serviceOptions, IWebHostEnvironment webHostEnvironment):base(fileAccessor, cultureOptions)
        {
            Data = new ConcurrentDictionary<string, List<Boss>>();
            FilePath = new Dictionary<string, string>();
            foreach (var language in Languages)
            {
                FilePath.Add(language, Path.Combine(webHostEnvironment.ContentRootPath, serviceOptions.Value?.BossDataService ?? "Data\\Bosses", $"{language}.json"));
            }
        }

        public async override Task Aggregate()
        {
            if (Data.Count > 0)
                return;
            foreach (var path in FilePath)
            {
                var result = await FileAccessor.JsonFileReader<List<Boss>>(path.Value);
                Data.TryAdd(path.Key, result);
            }
            await base.Aggregate();
        }
    }
}
