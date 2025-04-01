using MapleTools.Abstraction;
using MapleTools.Models.Boss;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Concurrent;
using MapleTools.Models.Content;
using System.Globalization;

namespace MapleTools.Services.FileDataServices
{
    public class DetailedArticleService<T>: FileDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, T>>> where T : IArticle
    {

        public DetailedArticleService(IFileAccessor fileAccessor, string name) : base(fileAccessor, name)
        {

        }

        public async override Task Aggregate()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            if (!Data.ContainsKey(culture))
            {
                    var contents = new ConcurrentDictionary<string, T>();
                    var contentPath = Directory.GetDirectories(FilePath);
                    foreach(var path in contentPath)
                    {
                        var result = await FileAccessor.JsonFileReader<T>(path, culture, 9999);
                        contents.TryAdd(result.ContentPath, result);
                    }
                    Data.TryAdd(culture, contents);
            }
            await base.Aggregate();
        }
    }
}
