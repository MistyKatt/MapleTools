using MapleTools.Services.FileDataServices;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace MapleTools.Controllers
{
    [Route("bosses")]
    public class BossController : Controller
    {
        private BossDataService _bossDataService;

        private string _filePath;

        public BossController(BossDataService bossDataService)
        {
            _bossDataService = bossDataService;
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            if(_bossDataService.Bosses.Count == 0)
            {
                await _bossDataService.Aggregate();
            }
            var language = CultureInfo.CurrentCulture.Name;
            _bossDataService.Bosses.TryGetValue(language, out var result);
            return View(result);
        }
    }
}
