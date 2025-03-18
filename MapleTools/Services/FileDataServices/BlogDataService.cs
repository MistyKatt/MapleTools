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

        public BlogDataService(IFileAccessor fileAccessor, string name) : base(fileAccessor, name)
        {
            
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
