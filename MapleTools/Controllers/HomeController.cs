using MapleTools.Models;
using MapleTools.Services.ApiDataServices;
using MapleTools.Services.FileDataServices;
using MapleTools.Simulation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MapleTools.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private BanListService _banListService;
        private TrendingService _trendingService;
        private FarmingService _farmingService;
        private ToolDataService _toolDataService;
        private BlogDataService _blogDataService;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(BanListService banListService, TrendingService trendingService, FarmingService farmingService, ToolDataService toolDataService, BlogDataService blogDataService, IWebHostEnvironment webHostEnvironment)
        {
            _banListService = banListService;
            _trendingService = trendingService;
            _farmingService = farmingService;
            _webHostEnvironment = webHostEnvironment;
            _toolDataService = toolDataService;
            _blogDataService = blogDataService;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View(DummyData.Articles);
        }

        [Route("BanList")]
        public async Task<IActionResult> BanList()
        {
            if(_banListService.Aggregated.Count == 0)
            {
                await _banListService.Aggregate();
            }
            var banlist = new BanList()
            {
                BannedPlayers = _banListService.Aggregated
            };
            return View(banlist);
        }
        [Route("Trending")]
        public async Task<IActionResult> Trending()
        {
            if (_trendingService.Aggregated.Count == 0)
            {
                await _trendingService.Aggregate();
            }
            var trending = new Trending()
            {
                JobTrending = _trendingService.Aggregated
            };
            return View(trending);
        }

        [Route("Farming")]
        public async Task<IActionResult> Farming()
        {
            if (_farmingService.Aggregated.Count == 0)
            {
                await _farmingService.Aggregate();
            }
            var farming = new Farming()
            {
                FarmingPlayers = _farmingService.Aggregated
            };
            return View(farming);
        }
        [Route("Tools")]
        public async Task<IActionResult> Tools()
        {
            if (_toolDataService.Tools.Count == 0)
            {
                await _toolDataService.Aggregate();
            }
            var language = CultureInfo.CurrentCulture.Name;
            _toolDataService.Tools.TryGetValue(language, out var tool);
            return View(tool);
        }
        [Route("Blogs")]
        public async Task<IActionResult> Blogs()
        {
            if (_blogDataService.Blogs.Count == 0)
            {
                await _blogDataService.Aggregate();
            }
            return View(_blogDataService.Blogs);
        }

    }
}
