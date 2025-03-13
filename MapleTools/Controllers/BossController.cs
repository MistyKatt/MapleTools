using MapleTools.Services.FileDataServices;
using Microsoft.AspNetCore.Mvc;

namespace MapleTools.Controllers
{
    [Route("bosses")]
    public class BossController : Controller
    {
        private BossDataService _bossDataService;

        private string _filePath;

        private IWebHostEnvironment _webHostEnvironment;
        public BossController(BossDataService bossDataService, IWebHostEnvironment webHostEnvironment)
        {
            _bossDataService = bossDataService;
            _webHostEnvironment = webHostEnvironment;
            _filePath = _webHostEnvironment.ContentRootPath + @"\Simulation\bosses.json";    
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            if(_bossDataService.Bosses.Count == 0)
            {
                await _bossDataService.Aggregate();
            }
            return View(_bossDataService.Bosses);
        }
    }
}
