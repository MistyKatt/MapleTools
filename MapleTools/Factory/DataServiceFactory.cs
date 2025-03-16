using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models;
using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using MapleTools.Services;
using MapleTools.Services.ApiDataServices;
using MapleTools.Services.FileDataServices;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace MapleTools.Factory
{
    public class DataServiceFactory
    {
        enum DataServiceType
        {
            BanListService,
            FarmingService,
            TrendingService,
            BlogDataService,
            BossDataService,
            ToolDataService
        }

        IOptions<ServiceOptions> _serviceOptions;

        IFileAccessor _fileAccessor;

        IWebHostEnvironment _webHostEnvironment;

        IOptions<LocalizationOptions> _localizationOptions;
        public DataServiceFactory(IOptions<ServiceOptions> serviceOptions, IFileAccessor fileAccessor, IWebHostEnvironment webHostEnvironment, IOptions<LocalizationOptions> localizationOptions) 
        { 
            _serviceOptions = serviceOptions;
            _localizationOptions = localizationOptions;
            _fileAccessor = fileAccessor;   
            _webHostEnvironment = webHostEnvironment;
        }

        public IDataService<Dictionary<string, List<Player>>> BanListInstance()
        {
            return new BanListService(_serviceOptions);
        }

        public IDataService<Dictionary<string, List<Player>>> FarmingInstance()
        {
            return new FarmingService(_serviceOptions);
        }

        public IDataService<Dictionary<string, List<(string, int)>>> TrendingInstance()
        {
            return new TrendingService(_serviceOptions);
        }

        public IDataService<ConcurrentDictionary<string, List<Blog>>> BlogDataInstance()
        {
            return new BlogDataService(_serviceOptions, _fileAccessor, _webHostEnvironment, _localizationOptions);
        }

        public IDataService<ConcurrentDictionary<string, List<Boss>>> BossDataInstance()
        {
            return new BossDataService(_localizationOptions, _fileAccessor, _serviceOptions, _webHostEnvironment);
        }

        public IDataService<ConcurrentDictionary<string, List<Tool>>> ToolDataInstance()
        {
            return new ToolDataService(_serviceOptions, _fileAccessor, _webHostEnvironment, _localizationOptions);
        }
    }
}
