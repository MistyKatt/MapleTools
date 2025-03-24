using MapleTools.Abstraction;
using MapleTools.Factory;
using MapleTools.Models;
using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using MapleTools.Models.Tool;
using MapleTools.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

namespace MapleTools.Controllers
{
    public enum EditMode
    {
        Create,
        Edit,
        Delete
    }

    public enum ServiceType
    {
        Tool,
        Boss,
        Blog
    }
    [Route("hidden/admin")]
    public class AdminController : Controller
    {


        DataServiceFactory _dataServiceFactory;
        private IDataService<ConcurrentDictionary<string, List<Blog>>> _blogDataService;
        private IDataService<ConcurrentDictionary<string, List<Tool>>> _toolDataService;
        private IDataService<ConcurrentDictionary<string, List<Boss>>> _bossDataService;
        private IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BossArticle>>> _bossArticleService;
        private IDataService<ConcurrentDictionary<string, ConcurrentDictionary<string, BlogArticle>>> _blogArticleService;
        private IFileAccessor _fileAccessor;
        private IOptions<Localization.LocalizationOptions> _localizationOptions;
        private IOptions<ServiceOptions> _serviceOptions;
        private IWebHostEnvironment _webHostEnvironment;
        public AdminController(DataServiceFactory dataServiceFactory, IOptions<Localization.LocalizationOptions> options, IOptions<ServiceOptions> serviceOptions, IFileAccessor fileAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _dataServiceFactory = dataServiceFactory;
            _localizationOptions = options;
            _serviceOptions = serviceOptions;
            _fileAccessor = fileAccessor;
            _blogDataService = _dataServiceFactory.GetBlogDataService();
            _toolDataService = _dataServiceFactory.GetToolDataService();
            _bossDataService = _dataServiceFactory.GetBossDataService();
            _bossArticleService = _dataServiceFactory.GetBossArticleService();
            _blogArticleService = _dataServiceFactory.BlogArticleService();
            _webHostEnvironment = webHostEnvironment;
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Route("bosses")]
        public async Task<IActionResult> Bosses()
        {
            if (_bossDataService.Data.Count == 0)
                await _bossDataService.Aggregate();
            return View(_bossDataService.Data[CultureInfo.CurrentCulture.Name]);
        }
        [Route("editboss")]
        [HttpGet]
        public async Task<IActionResult> EditBoss(string id, EditMode mode)
        {
            if (_bossDataService.Data.Count == 0)
                await _bossDataService.Aggregate();
            var bosses = _bossDataService.Data[CultureInfo.CurrentCulture.Name];
            var boss = bosses.FirstOrDefault(t => t.Id == id);
            if (boss == null)
                boss = bosses.FirstOrDefault();
            ViewData["mode"] = mode;
            return View("~/Views/Admin/Bosses/EditBoss.cshtml", boss);
        }
        [HttpPost]
        [Route("editboss")]
        public async Task<IActionResult> EditBoss(Boss boss, EditMode mode)
        {
            var result = ProcessEntity<Boss>(
                    boss,
                    mode,
                    () => _bossDataService.Data[CultureInfo.CurrentCulture.Name]);
            await PersistChange<List<Boss>>(
                    Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value.BossDataService ?? "Data\\Tools"),
                    result,
                    () => _bossDataService.Data.Clear()
                );
            return RedirectToAction("Bosses", "Admin");
        }

        [Route("editbossarticle")]
        public async Task<IActionResult> EditBossArticle(string id, string path)
        {
            if (_bossArticleService.Data.Count == 0)
                await _bossArticleService.Aggregate();
            var bosses = _bossArticleService.Data[CultureInfo.CurrentCulture.Name];
            bosses.TryGetValue(path, out var bossArticle);
            if (bossArticle == null)
            {
                bossArticle = new BossArticle()
                {
                    Id = id,
                    ContentPath = path,
                    HtmlContent = "<p>Placeholder, Edit Here</p>"
                };
            }
            return View("~/Views/Admin/Bosses/EditBossArticle.cshtml", bossArticle);
        }

        [HttpPost]
        [Route("editbossarticle")]
        public async Task<IActionResult> EditBossArticle(BossArticle article, EditMode mode)
        {
            var rootPath = Path.Combine(_webHostEnvironment.ContentRootPath,_serviceOptions.Value.BossDataService??"Data\\Bosses", article.ContentPath);
            await PersistChange<BossArticle>(
                    rootPath,
                    article,
                    () => _blogArticleService.Data.Clear()
                );
        
            return RedirectToAction("Bosses", "Admin");
        }


        [Route("blogs")]
        public async Task<IActionResult> Blogs()
        {
            if (_blogDataService.Data.Count == 0)
                await _blogDataService.Aggregate();
            return View(_blogDataService.Data[CultureInfo.CurrentCulture.Name]);
        }
        [Route("editblog")]
        public async Task<IActionResult> EditBlog(string id, EditMode mode)
        {
            if (_blogDataService.Data.Count == 0)
                await _blogDataService.Aggregate();
            var blogs = _blogDataService.Data[CultureInfo.CurrentCulture.Name];
            var blog = blogs.FirstOrDefault(t => t.Id == id);
            if (blog == null)
                blog = blogs.FirstOrDefault();
            ViewData["mode"] = mode;
            return View("~/Views/Admin/Blogs/EditBlog.cshtml", blog);
        }
        [HttpPost]
        [Route("editblog")]
        public async Task<IActionResult> EditBlog(Blog blog, EditMode mode)
        {
            var result = ProcessEntity<Blog>(
                    blog,
                    mode,
                    () => _blogDataService.Data[CultureInfo.CurrentCulture.Name]);
            await PersistChange<List<Blog>>(
                    Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value.ToolDataService ?? "Data\\Blogs"),
                    result,
                    () => _blogDataService.Data.Clear()
                );
            return RedirectToAction("Blogs", "Admin");
        }

        [Route("editblogarticle")]
        public async Task<IActionResult> EditBlogArticle(string id, string path)
        {
            if (_blogArticleService.Data.Count == 0)
                await _blogArticleService.Aggregate();
            var blogs = _blogArticleService.Data[CultureInfo.CurrentCulture.Name];
            blogs.TryGetValue(path, out var blogArticle);
            if (blogArticle == null)
            {
                blogArticle = new BlogArticle()
                {
                    Id = id,
                    ContentPath = path,
                    HtmlContent = "<p>Placeholder, Edit Here</p>"
                };
            }
            return View("~/Views/Admin/Blogs/EditBlogArticle.cshtml", blogArticle);
        }

        [HttpPost]
        [Route("editblogarticle")]
        public async Task<IActionResult> EditBlogArticle(BossArticle article, EditMode mode)
        {
            var rootPath = Path.Combine(_webHostEnvironment.ContentRootPath,_serviceOptions.Value.BlogDataService??"Data\\Blog", article.ContentPath);
            await PersistChange<BossArticle>(
                    rootPath,
                    article,
                    () => _blogArticleService.Data.Clear()
                );

            return RedirectToAction("Blogs", "Admin");
        }
        [Route("tools")]
        public async Task<IActionResult> Tools()
        {
            if (_toolDataService.Data.Count == 0)
                await _toolDataService.Aggregate();
            return View(_toolDataService.Data[CultureInfo.CurrentCulture.Name]);
        }
        [Route("edittool")]
        [HttpGet]
        public async Task<IActionResult> EditTool(string id, EditMode mode)
        {
            if (_toolDataService.Data.Count == 0)
                await _toolDataService.Aggregate();
            var tools = _toolDataService.Data[CultureInfo.CurrentCulture.Name];
            var tool = tools.FirstOrDefault(t => t.Id == id);
            if (tool == null)
                tool = tools.FirstOrDefault();
            ViewData["mode"] = mode;
            return View("~/Views/Admin/Tools/EditTool.cshtml", tool);
        }
        [HttpPost]
        [Route("edittool")]
        public async Task<IActionResult> EditTool(Tool tool, EditMode mode)
        {
            var result = ProcessEntity<Tool>(
                    tool,
                    mode,
                    () => _toolDataService.Data[CultureInfo.CurrentCulture.Name]);
            await PersistChange<List<Tool>>(
                    Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value.ToolDataService ?? "Data\\Tools"),
                    result,
                    () => _toolDataService.Data.Clear()
                );
            return RedirectToAction("Tools", "Admin");
        }

        private async Task<T> GenerateEntity<T>(
            Func<Task> aggregate,
            Func<List<T>> getData,
            string id
            ) where T:IdBasedModel
        {
            await aggregate();
            var result = getData();
            var model = result.FirstOrDefault(t => t.Id == id);
            if (model == null)
                model = result.FirstOrDefault();
            return model;
        }

        private List<T> ProcessEntity<T>(
        T entity,
        EditMode mode,
        Func<List<T>> getData
        ) where T : IdBasedModel //process stored data modification
        {
            var data = getData();

            if (mode == EditMode.Edit)
            {
                var index = data.FindIndex(t => t.Id == entity.Id);
                if (index != -1)
                {
                    data[index] = entity;

                }
            }
            else if (mode == EditMode.Create)
            {
                data.Add(entity);
            }
            else if (mode == EditMode.Delete)
            {
                var remainedTools = data.Where(t => t.Id != entity.Id).ToList();
                data = remainedTools;
            }

            return data;
        }

        private async Task PersistChange<T>(string filePath, T result, Action ClearCache)
        {
            await _fileAccessor.JsonFileWriter<T>(filePath, result);
            ClearCache();
        }



    }
}
