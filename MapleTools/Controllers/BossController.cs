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
        private IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BossArticle>>> _bossArticleService;

        private DataServiceFactory _dataServiceFactory;

        private string _filePath;

        public BossController(DataServiceFactory dataServiceFactory)
        {
            _dataServiceFactory = dataServiceFactory;
            _bossDataService = dataServiceFactory.GetBossDataService();
            _bossArticleService = _dataServiceFactory.GetBossArticleService();
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

        [Route("{articleName}")]
        public async Task<IActionResult> BossArticle(string articleName)
        {
            if (_bossArticleService.Data.Count == 0)
            {
                await _bossArticleService.Aggregate();
            }
            var language = CultureInfo.CurrentCulture.Name;
            _bossArticleService.Data.TryGetValue(language, out var bosses);
            bosses.TryGetValue(articleName, out var article);
            return View(article);
        }
    }
}
