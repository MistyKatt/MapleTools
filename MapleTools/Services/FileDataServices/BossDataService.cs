﻿using MapleTools.Abstraction;
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

        public BossDataService(IFileAccessor fileAccessor, string name):base(fileAccessor, name)
        {
            
        }

        public async override Task Aggregate()
        {
            if (Data.Count == 0)
            {
                foreach (var language in Languages)
                {
                    var result = await FileAccessor.JsonFileReader<List<Boss>>(FilePath, language);
                    Data.TryAdd(language, result);
                }
            }
            await base.Aggregate();
        }
    }
}
