using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models.Tool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MapleTools.Services.FileDataServices
{
    public class ToolDataService : FileDataService<ConcurrentDictionary<string, List<Tool>>>
    {

        public ToolDataService(IFileAccessor fileAccessor, string name) : base(fileAccessor, name)
        {

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

        public override void Clear()
        {
            Data.Clear();
        }
    }
}
