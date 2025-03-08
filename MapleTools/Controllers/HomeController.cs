using MapleTools.Models;
using MapleTools.Services.Aggregator;
using MapleTools.Simulation;
using Microsoft.AspNetCore.Mvc;

namespace MapleTools.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private BanListAggregator _banListAggregator;
        private TrendingAggregator _trendingAggregator;
        private FarmingAggregator _farmingAggregator;

        public HomeController(BanListAggregator banListAggregator, TrendingAggregator trendingAggregator, FarmingAggregator farmingAggregator)
        {
            _banListAggregator = banListAggregator;
            _trendingAggregator = trendingAggregator;
            _farmingAggregator = farmingAggregator;
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
        public IActionResult Tools()
        {
            return View();
        }
        [Route("Blogs")]
        public IActionResult Blogs()
        {
            return View();
        }

    }
}
