using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MapleTools.Services.FileDataServices
{
    public class BlogDataService : FileDataService<ConcurrentDictionary<string, List<Blog>>>
    {

        public BlogDataService(IOptions<ServiceOptions> serviceOptions,IFileAccessor fileAccessor, IWebHostEnvironment webHostEnvironment, IOptions<LocalizationOptions> options) : base(fileAccessor, options)
        {
            Data = new ConcurrentDictionary<string, List<Blog>>();
            FilePath = new Dictionary<string, string>();
            foreach (var language in Languages)
            {
                FilePath.Add(language, Path.Combine(webHostEnvironment.ContentRootPath, serviceOptions.Value?.BlogDataService ?? "dummy"));
            }
        }

        public async override Task Aggregate()
        {
            if (Data.Count > 0)
                return;
            foreach (var path in FilePath)
            {
                var result = await FileAccessor.JsonFileReader<List<Blog>>(path.Value);
                Data.TryAdd(path.Key, result);
            }
            await base.Aggregate();
        }
    }
}
