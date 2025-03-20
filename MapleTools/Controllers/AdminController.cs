using MapleTools.Abstraction;
using MapleTools.Factory;
using MapleTools.Localization;
using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using MapleTools.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Globalization;

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
        private IFileAccessor _fileAccessor;
        private IOptions<Localization.LocalizationOptions> _localizationOptions;
        private IOptions<ServiceOptions> _serviceOptions;
        private IWebHostEnvironment _webHostEnvironment;
        public AdminController(DataServiceFactory dataServiceFactory, IOptions<Localization.LocalizationOptions> options,IOptions<ServiceOptions> serviceOptions, IFileAccessor fileAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _dataServiceFactory = dataServiceFactory;
            _localizationOptions = options;
            _serviceOptions = serviceOptions;
            _fileAccessor = fileAccessor;
            _blogDataService = _dataServiceFactory.GetBlogDataService();
            _toolDataService = _dataServiceFactory.GetToolDataService();    
            _bossDataService = _dataServiceFactory.GetBossDataService();
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
            var data = _bossDataService.Data[CultureInfo.CurrentCulture.Name];
            if (mode == EditMode.Edit)
            {
                var index = data.FindIndex(t => t.Id == boss.Id);
                if (index != -1)
                {
                    data[index] = boss;

                }
            }
            else if (mode == EditMode.Create)
            {
                data.Add(boss);
            }
            else if (mode == EditMode.Delete)
            {
                data = _bossDataService.Data[CultureInfo.CurrentCulture.Name];
                var remainedBosses = data.Where(t => t.Id != boss.Id).ToList();
                data = remainedBosses;
            }
            else
            {
                return RedirectToAction("Bosses", "Admin");
            }
            var rootPath = Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value.BossDataService ?? "Data\\Bosses");
            await _fileAccessor.JsonFileWriter<List<Boss>>(rootPath, data);
            _bossDataService.Data.Clear();
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
            var data = _blogDataService.Data[CultureInfo.CurrentCulture.Name];
            if (mode == EditMode.Edit)
            {
                var index = data.FindIndex(t => t.Id == blog.Id);
                if (index != -1)
                {
                    data[index] = blog;

                }
            }
            else if (mode == EditMode.Create)
            {
                data.Add(blog);
            }
            else if (mode == EditMode.Delete)
            {
                data = _blogDataService.Data[CultureInfo.CurrentCulture.Name];
                var remainedBosses = data.Where(t => t.Id != blog.Id).ToList();
                data = remainedBosses;
            }
            else
            {
                return RedirectToAction("Blogs", "Admin");
            }
            var rootPath = Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value.BlogDataService ?? "Data\\Blogs");
            await _fileAccessor.JsonFileWriter<List<Blog>>(rootPath, data);
            _blogDataService.Data.Clear();
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
            var tool = tools.FirstOrDefault(t=>t.Id == id);
            if (tool == null)
                tool = tools.FirstOrDefault();
            ViewData["mode"] = mode;
            return View("~/Views/Admin/Tools/EditTool.cshtml", tool);
        }
        [HttpPost]
        [Route("edittool")]
        public async Task<IActionResult> EditTool(Tool tool,EditMode mode)
        {
            var data = _toolDataService.Data[CultureInfo.CurrentCulture.Name];
            if (mode == EditMode.Edit)
            {
                var index = data.FindIndex(t => t.Id == tool.Id);
                if (index != -1)
                {
                    data[index] = tool;

                }
            }
            else if(mode == EditMode.Create)
            {
                data.Add(tool);
            }
            else if(mode == EditMode.Delete)
            {
                data = _toolDataService.Data[CultureInfo.CurrentCulture.Name];
                var remainedTools = data.Where(t => t.Id != tool.Id).ToList();
                data = remainedTools;
            }
            else
            {
                return RedirectToAction("Tools", "Admin");
            }
            var rootPath = Path.Combine(_webHostEnvironment.ContentRootPath, _serviceOptions.Value.ToolDataService ?? "Data\\Tools");
            await _fileAccessor.JsonFileWriter<List<Tool>>(rootPath, data);
            _toolDataService.Data.Clear();
            return RedirectToAction("Tools", "Admin");
        }     


    }
}
