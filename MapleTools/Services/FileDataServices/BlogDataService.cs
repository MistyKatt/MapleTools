using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MapleTools.Services.FileDataServices
{
    public class BlogDataService:FileDataService
    {
        private Dictionary<int, List<Blog>> _blogs;

        private string _filePath;

        public BlogDataService(IOptions<ServiceOptions> serviceOptions, IWebHostEnvironment webHostEnvironment, IOptions<LocalizationOptions> options) : base()
        {
            _blogs = new Dictionary<int, List<Blog>>();
            _filePath = Path.Combine(webHostEnvironment.ContentRootPath, serviceOptions.Value?.BlogService ?? "dummy");

        }

        public Dictionary<int, List<Blog>> Blogs { get { return _blogs; } }

        public async override Task Aggregate()
        {
            if (Blogs.Count > 0)
                return;
            if (File.Exists(_filePath) && _filePath.EndsWith("json"))
            {
                using (StreamReader rs = new StreamReader(_filePath))
                {
                    var result = await rs.ReadToEndAsync();
                    var blogs = JsonConvert.DeserializeObject<List<Blog>>(result) ?? new List<Blog>();
                    _blogs = blogs
                        .GroupBy(b => b.Stage)
                        .ToDictionary
                        (
                            b => b.Key,
                            b => b.ToList()
                        );
                }
            }
            await base.Aggregate();
        }
    }
}
