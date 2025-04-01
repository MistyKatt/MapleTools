using MapleTools.Abstraction;
using MapleTools.Models.Boss;
using System.Collections.Concurrent;
using System.Globalization;

namespace MapleTools.Services.FileDataServices
{
    public class BossDataService : FileDataService<ConcurrentDictionary<string, List<Boss>>>
    {

        public BossDataService(IFileAccessor fileAccessor, string name) : base(fileAccessor, name)
        {

        }

        public async override Task Aggregate()
        {
            var culture = CultureInfo.CurrentCulture.Name;
            if (!Data.ContainsKey(culture))
            {

                var result = await FileAccessor.JsonFileReader<List<Boss>>(FilePath, culture, 9999);
                Data.TryAdd(culture, result);
            }
            await base.Aggregate();
        }
    }
}
