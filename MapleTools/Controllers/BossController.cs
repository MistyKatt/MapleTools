using MapleTools.Abstraction;
using MapleTools.Factory;
using MapleTools.Models.Boss;
using MapleTools.Services.FileDataServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Globalization;

namespace MapleTools.Controllers
{
    [Route("bosses")]
    public class BossController : Controller
    {

        private IDataService<ConcurrentDictionary<string, List<Boss>>> _bossDataService;

        private DataServiceFactory _dataServiceFactory;

        private string _filePath;

        public BossController(DataServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
            _bossDataService = dataServiceFactory.GetBossDataService();
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            if(_bossDataService.Data.Count == 0)
            {
                await _bossDataService.Aggregate();
            }
            var language = CultureInfo.CurrentCulture.Name;
            _bossDataService.Data.TryGetValue(language, out var result);
            return View(result);
        }
    }
}
