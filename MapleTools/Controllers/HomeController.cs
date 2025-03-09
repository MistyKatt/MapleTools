using MapleTools.Models;
using MapleTools.Services.Aggregator;
using MapleTools.Services.BossDataService;
using MapleTools.Simulation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MapleTools.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private BanListAggregator _banListAggregator;
        private TrendingAggregator _trendingAggregator;
        private FarmingAggregator _farmingAggregator;
        private ToolDataService _toolDataService;
        private BlogDataService _blogDataService;
        private string _filePathTool, _filePathBlog;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(BanListAggregator banListAggregator, TrendingAggregator trendingAggregator, FarmingAggregator farmingAggregator, ToolDataService toolDataService, BlogDataService blogDataService, IWebHostEnvironment webHostEnvironment)
        {
            _banListAggregator = banListAggregator;
            _trendingAggregator = trendingAggregator;
            _farmingAggregator = farmingAggregator;
            _webHostEnvironment = webHostEnvironment;
            _toolDataService = toolDataService;
            _blogDataService = blogDataService;
            _filePathTool = _webHostEnvironment.ContentRootPath + @"\Simulation\tools.json";
            _filePathBlog = _webHostEnvironment.ContentRootPath + @"\Simulation\blogs.json";
        }

        [Route("")]
        public IActionResult Index()
        {
            return View(DummyData.Articles);
        }

        [Route("BanList")]
        public IActionResult BanList()
        {
            if(_banListAggregator.Aggregated.Count == 0)
            {
                _banListAggregator.Aggregate();
            }
            var banlist = new BanList()
            {
                BannedPlayers = _banListAggregator.Aggregated
            };
            return View(banlist);
        }
        [Route("Trending")]
        public IActionResult Trending()
        {
            if (_trendingAggregator.Aggregated.Count == 0)
            {
                _trendingAggregator.Aggregate();
            }
            var trending = new Trending()
            {
                JobTrending = _trendingAggregator.Aggregated
            };
            return View(trending);
        }

        [Route("Farming")]
        public IActionResult Farming()
        {
            if (_farmingAggregator.Aggregated.Count == 0)
            {
                _farmingAggregator.Aggregate();
            }
            var farming = new Farming()
            {
                FarmingPlayers = _farmingAggregator.Aggregated
            };
            return View(farming);
        }
        [Route("Tools")]
        public async Task<IActionResult> Tools()
        {
            if (_toolDataService.Tools.Count == 0)
            {
                await _toolDataService.GetToolData(_filePathTool);
            }
            return View(_toolDataService.Tools);
        }
        [Route("Blogs")]
        public async Task<IActionResult> Blogs()
        {
            if (_blogDataService.Blogs.Count == 0)
            {
                await _blogDataService.GetBlogData(_filePathBlog);
            }
            return View(_blogDataService.Blogs);
        }

    }
}
