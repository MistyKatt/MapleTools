using MapleTools.Abstraction;
using MapleTools.Factory;
using MapleTools.Models;
using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using MapleTools.Services.ApiDataServices;
using MapleTools.Services.FileDataServices;
using MapleTools.Simulation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Globalization;

namespace MapleTools.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private DataServiceFactory _dataServiceFactory;
        private IDataService<Dictionary<string, List<Player>>> _banListService;
        private IDataService<Dictionary<string, List<Player>>> _farmingService;
        private IDataService<Dictionary<string, List<(string, int)>>> _trendingService;
        private IDataService<ConcurrentDictionary<string, List<Blog>>> _blogDataService;       
        private IDataService<ConcurrentDictionary<string, List<Tool>>> _toolDataService;

        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(DataServiceFactory dataServiceFactory, IWebHostEnvironment webHostEnvironment)
        {

            _dataServiceFactory = dataServiceFactory;
            _banListService = _dataServiceFactory.BanListInstance();
            _trendingService = _dataServiceFactory.TrendingInstance();
            _farmingService = _dataServiceFactory.FarmingInstance();
            _webHostEnvironment = webHostEnvironment;
            _toolDataService = _dataServiceFactory.ToolDataInstance();
            _blogDataService = _dataServiceFactory.BlogDataInstance();
        }

        [Route("")]
        public IActionResult Index()
        {
            return View(DummyData.Articles);
        }

        [Route("BanList")]
        public async Task<IActionResult> BanList()
        {
            if(_banListService.Data.Count == 0)
            {
                await _banListService.Aggregate();
            }
            var banlist = new BanList()
            {
                BannedPlayers = _banListService.Data
            };
            return View(banlist);
        }
        [Route("Trending")]
        public async Task<IActionResult> Trending()
        {
            if (_trendingService.Data.Count == 0)
            {
                await _trendingService.Aggregate();
            }
            var trending = new Trending()
            {
                JobTrending = _trendingService.Data
            };
            return View(trending);
        }

        [Route("Farming")]
        public async Task<IActionResult> Farming()
        {
            if (_farmingService.Data.Count == 0)
            {
                await _farmingService.Aggregate();
            }
            var farming = new Farming()
            {
                FarmingPlayers = _farmingService.Data
            };
            return View(farming);
        }
        [Route("Tools")]
        public async Task<IActionResult> Tools()
        {
            if (_toolDataService.Data.Count == 0)
            {
                await _toolDataService.Aggregate();
            }
            var language = CultureInfo.CurrentCulture.Name;
            _toolDataService.Data.TryGetValue(language, out var tool);
            return View(tool);
        }
        [Route("Blogs")]
        public async Task<IActionResult> Blogs()
        {
            if (_blogDataService.Data.Count == 0)
            {
                await _blogDataService.Aggregate();
            }
            var language = CultureInfo.CurrentCulture.Name;
            _blogDataService.Data.TryGetValue(language, out var blog);
            var result = blog?.GroupBy(b=>b.Stage).ToDictionary(g=>g.Key, g=>g.ToList());
            return View(result);
        }

    }
}
