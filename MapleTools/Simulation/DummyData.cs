using MapleTools.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MapleTools.Simulation
{
    public static class DummyData
    {
        public static List<Article> Articles = new List<Article>()
        {
            new Article()
            {
                Id = 1,
                Content="article one",
                Title="first article" ,
                Type=ArticleType.Personal
                
            },
            new Article()
            {
                Id = 2,
                Content="article two",
                Title="second article",
                Type=ArticleType.Personal
            },
            new Article()
            {
                Id = 1,
                Content="article three",
                Title="third article",
                Type=ArticleType.News
            }
        };

        public static List<Player> Players = new List<Player>();

        public static async void GeneratePlayers(string filePath)
        {
            try
            {
                if (File.Exists(filePath) && filePath.EndsWith("json"))
                {
                    using (StreamReader rs = new StreamReader(filePath))
                    {
                        var result = await rs.ReadToEndAsync();
                        Players = JsonConvert.DeserializeObject<List<Player>>(result)??new List<Player>();
                    }
                }
                else
                {
                    //_logger.LogError($"The file open failed ");
                    Console.WriteLine($"The file open failed ");
                }
            }
            catch (IOException ex)
            {
                //_logger.LogError($"The file open failed {ex}");
                Console.WriteLine($"The file open failed {ex}");
            }
            
        }

        
    }
}
