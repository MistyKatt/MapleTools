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

        public static List<Player> BannedPlayers = new List<Player>();

        public static List<Player> FarmingPlayers = new List<Player>();

        public static async void GeneratePlayers(string filePath1, string filePath2, string filePath3)
        {
            try
            {
                if (File.Exists(filePath1) && filePath1.EndsWith("json"))
                {
                    using (StreamReader rs = new StreamReader(filePath1))
                    {
                        var result = await rs.ReadToEndAsync();
                        Players = JsonConvert.DeserializeObject<List<Player>>(result)??new List<Player>();
                    }
                }
                if (File.Exists(filePath2) && filePath2.EndsWith("json"))
                {
                    using (StreamReader rs = new StreamReader(filePath2))
                    {
                        var result = await rs.ReadToEndAsync();
                        BannedPlayers = JsonConvert.DeserializeObject<List<Player>>(result) ?? new List<Player>();
                    }
                }
                if (File.Exists(filePath3) && filePath3.EndsWith("json"))
                {
                    using (StreamReader rs = new StreamReader(filePath3))
                    {
                        var result = await rs.ReadToEndAsync();
                        FarmingPlayers = JsonConvert.DeserializeObject<List<Player>>(result) ?? new List<Player>();
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
