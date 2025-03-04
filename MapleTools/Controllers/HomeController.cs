using MapleTools.Simulation;
using Microsoft.AspNetCore.Mvc;

namespace MapleTools.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            return View(DummyData.Articles);
        }

        [Route("BanList")]
        public IActionResult BanList()
        {
            return View();
        }
        [Route("Trending")]
        public IActionResult Trending()
        {
            return View();
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
