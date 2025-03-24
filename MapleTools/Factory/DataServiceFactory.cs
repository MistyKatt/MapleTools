using MapleTools.Abstraction;
using MapleTools.Controllers;
using MapleTools.Localization;
using MapleTools.Models.Api;
using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using MapleTools.Models.Tool;
using MapleTools.Services;
using MapleTools.Services.ApiDataServices;
using MapleTools.Services.FileDataServices;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MapleTools.Factory
{
    public class DataServiceFactory
    {
        public enum DataServiceType
        {
            BanListService,
            FarmingService,
            TrendingService,
            BlogDataService,
            BossDataService,
            ToolDataService,
            BlogArticleService,
            BossArticleService
        }

        IOptions<ServiceOptions> _serviceOptions;

        IFileAccessor _fileAccessor;

        IWebHostEnvironment _webHostEnvironment;

        IOptions<LocalizationOptions> _localizationOptions;

        private readonly Lazy<IDataService<Dictionary<string, List<Player>>>> _banListService;
        private readonly Lazy<IDataService<Dictionary<string, List<Player>>>> _farmingService;
        private readonly Lazy<IDataService<Dictionary<string, List<(string, int)>>>> _trendingService;
        private readonly Lazy<IDataService<ConcurrentDictionary<string, List<Blog>>>> _blogDataService;
        private readonly Lazy<IDataService<ConcurrentDictionary<string, List<Boss>>>> _bossDataService;
        private readonly Lazy<IDataService<ConcurrentDictionary<string, List<Tool>>>> _toolDataService;
        private readonly Lazy<IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BossArticle>>>> _bossArticleService;
        private readonly Lazy<IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BlogArticle>>>> _blogArticleService;
        private Dictionary<string, Type> _serviceTypes;
        public DataServiceFactory(IOptions<ServiceOptions> serviceOptions, IFileAccessor fileAccessor, IWebHostEnvironment webHostEnvironment, IOptions<LocalizationOptions> localizationOptions) 
        { 
            _serviceOptions = serviceOptions;
            _localizationOptions = localizationOptions;
            _fileAccessor = fileAccessor;   
            _webHostEnvironment = webHostEnvironment;

            _banListService = new Lazy<IDataService<Dictionary<string, List<Player>>>>(() => BanListInstance());
            _farmingService = new Lazy<IDataService<Dictionary<string, List<Player>>>>(() => FarmingInstance());
            _trendingService = new Lazy<IDataService<Dictionary<string, List<(string, int)>>>>(() => TrendingInstance());
            _blogDataService = new Lazy<IDataService<ConcurrentDictionary<string, List<Blog>>>>(() => BlogDataInstance());
            _bossDataService = new Lazy<IDataService<ConcurrentDictionary<string, List<Boss>>>>(() => BossDataInstance());
            _toolDataService = new Lazy<IDataService<ConcurrentDictionary<string, List<Tool>>>>(() => ToolDataInstance());
            _bossArticleService = new Lazy<IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BossArticle>>>>(() => BossArticleInstance());
            _blogArticleService = new Lazy<IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BlogArticle>>>>(() => BlogArticleInstance());

        }

        public IDataService<Dictionary<string, List<Player>>> BanListInstance()
        {
            return new BanListService(_serviceOptions,"BanList");
        }

        public IDataService<Dictionary<string, List<Player>>> GetBanListService() => _banListService.Value;

        public IDataService<Dictionary<string, List<Player>>> FarmingInstance()
        {
            return new FarmingService(_serviceOptions, "Farming");
        }

        public IDataService<Dictionary<string, List<Player>>> GetFarmingService() => _farmingService.Value;

        public IDataService<Dictionary<string, List<(string, int)>>> TrendingInstance()
        {
            return new TrendingService(_serviceOptions, "Trending");
        }

        public IDataService<Dictionary<string, List<(string, int)>>> GetTrendingService() => _trendingService.Value;
        public IDataService<ConcurrentDictionary<string, List<Blog>>> BlogDataInstance()
        {
            var service = new BlogDataService(_fileAccessor, "BlogData");
            service.Data = new ConcurrentDictionary<string, List<Blog>>();
            service.Languages = _localizationOptions.Value.Languages;
            service.FilePath = Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value?.BlogDataService ?? "Data\\Blogs");
            return service;
        }

        public IDataService<ConcurrentDictionary<string, List<Blog>>> GetBlogDataService() => _blogDataService.Value;
        public IDataService<ConcurrentDictionary<string, List<Boss>>> BossDataInstance()
        {
            var service = new BossDataService(_fileAccessor, "BossData");
            service.Data = new ConcurrentDictionary<string, List<Boss>>();
            service.Languages = _localizationOptions.Value.Languages;
            service.FilePath = Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value?.BossDataService ?? "Data\\Bosses");
            return service;
        }

        public IDataService<ConcurrentDictionary<string, List<Boss>>> GetBossDataService() => _bossDataService.Value;
        public IDataService<ConcurrentDictionary<string, List<Tool>>> ToolDataInstance()
        {
            var service = new ToolDataService(_fileAccessor, "ToolData");
            service.Data = new ConcurrentDictionary<string, List<Tool>>();
            service.Languages = _localizationOptions.Value.Languages;
            service.FilePath = Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value?.ToolDataService ?? "Data\\Tools");
            return service;
        }

        public IDataService<ConcurrentDictionary<string, List<Tool>>> GetToolDataService() => _toolDataService.Value;

        public IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BossArticle>>> BossArticleInstance()
        {
            var service = new DetailedArticleService<BossArticle>(_fileAccessor, "BossDetail");
            service.Data = new ConcurrentDictionary<string, ConcurrentDictionary<string, BossArticle>>();
            service.Languages = _localizationOptions.Value.Languages;
            service.FilePath = Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value?.BossDataService ?? "Data\\Bosses");
            return service;
        }

        public IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BossArticle>>> GetBossArticleService() => _bossArticleService.Value;

        public IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BlogArticle>>> BlogArticleInstance()
        {
            var service = new DetailedArticleService<BlogArticle>(_fileAccessor, "BlogDetail");
            service.Data = new ConcurrentDictionary<string, ConcurrentDictionary<string, BlogArticle>>();
            service.Languages = _localizationOptions.Value.Languages;
            service.FilePath = Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value?.BlogDataService ?? "Data\\Blogs");
            return service;
        }

        public IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BlogArticle>>> GetBlogArticleService() => _blogArticleService.Value;


    }
}
