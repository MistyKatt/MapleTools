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
            if (_toolDataService.Data.Count == 0)
               await _toolDataService.Aggregate();

            return View(_toolDataService.Data[CultureInfo.CurrentCulture.Name]);
        }
        [Route("bosses")]
        public IActionResult Bosses()
        {
            return View();
        }
        [Route("blogs")]
        public IActionResult Blogs()
        {
            return View();
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
        public async Task<IActionResult> EditTool(string id, int mode)
        {
            if (_toolDataService.Data.Count == 0)
                await _toolDataService.Aggregate();
            var tools = _toolDataService.Data[CultureInfo.CurrentCulture.Name];
            var tool = tools.FirstOrDefault(t=>t.Id == id);
            ViewData["mode"] = (mode == 0 || mode == 1) ? mode : 1;
            if(mode == 0)
                return View("~/Views/Admin/Tools/EditTool.cshtml",new Tool() { Id=Guid.NewGuid().ToString(), Description="new one", IsExternal=true,Name="new one"});
            else
                return View("~/Views/Admin/Tools/EditTool.cshtml", tool);
        }
        [HttpPost]
        [Route("edittool")]
        public async Task<IActionResult> EditTool(Tool tool,int mode)
        {
            var data = _toolDataService.Data[CultureInfo.CurrentCulture.Name];
            if (mode == 1)
            {
                var index = data.FindIndex(t => t.Id == tool.Id);
                if (index != -1)
                {
                    data[index] = tool;

                }
            }
            else if(mode == 0)
            {
                data.Add(tool);
            }
            else if(mode == 2)
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
