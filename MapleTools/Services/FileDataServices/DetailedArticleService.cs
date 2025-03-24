using MapleTools.Abstraction;
using MapleTools.Models.Boss;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Concurrent;
using MapleTools.Models.Content;

namespace MapleTools.Services.FileDataServices
{
    public class DetailedArticleService<T>: FileDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, T>>> where T : IArticle
    {

        public DetailedArticleService(IFileAccessor fileAccessor, string name) : base(fileAccessor, name)
        {

        }

        public async override Task Aggregate()
        {
            if (Data.Count == 0)
            {
                foreach (var keyValuePair in ContentPath)
                {
                    var contents = new ConcurrentDictionary<string, T>();
                    foreach(var path in keyValuePair.Value)
                    {
                        var result = await FileAccessor.JsonFileReader<T>(path);
                        contents.TryAdd(result.ContentPath, result);
                    }
                    Data.TryAdd(keyValuePair.Key, contents);
                }
            }
            await base.Aggregate();
        }
    }
}
