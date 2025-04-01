using MapleTools.Abstraction;
using MapleTools.Models.Content;
using System.Collections.Concurrent;
using System.Globalization;

namespace MapleTools.Services.FileDataServices
{
    public class BlogDataService : FileDataService<ConcurrentDictionary<string, List<Blog>>>
    {

        public BlogDataService(IFileAccessor fileAccessor, string name) : base(fileAccessor, name)
        {

        }

        public async override Task Aggregate()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            if (!Data.ContainsKey(culture))
            {
                var result = await FileAccessor.JsonFileReader<List<Blog>>(FilePath, culture, 9999);
                Data.TryAdd(culture, result);

            }
            await base.Aggregate();
        }

    }
}
