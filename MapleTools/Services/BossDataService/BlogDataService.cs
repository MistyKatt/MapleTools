using MapleTools.Models.Boss;
using MapleTools.Models.Content;
using Newtonsoft.Json;

namespace MapleTools.Services.BossDataService
{
    public class BlogDataService
    {
        private Dictionary<int, List<Blog>> _blogs;

        public BlogDataService()
        {
            _blogs = new Dictionary<int, List<Blog>>();
        }

        public Dictionary<int, List<Blog>> Blogs { get { return _blogs; } }

        public async Task GetBlogData(string filePath)
        {
            if (Blogs.Count > 0)
                return;
            if (File.Exists(filePath) && filePath.EndsWith("json"))
            {
                using (StreamReader rs = new StreamReader(filePath))
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
        }
    }
}
