using MapleTools.Models.Api;
using Newtonsoft.Json;

namespace DataGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var players = new List<Player>();
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            //params
            string world = "eu";
            int sampleSize = 50;

            // Generate 1000 fake entries
            for (int i = 0; i < sampleSize; i++)
            {
                string worldName;
                if (world == "eu")
                    worldName = "solis";
                else
                {
                    int flip = random.Next(0, 2);
                    worldName = (flip == 0) ? "kronos" : "hyperion";
                }
                players.Add(new Player
                {
                    CharacterID = i,
                    CharacterName = $"test{random.Next(10000, 99999)}",
                    Level = random.Next(280, 296), // 295 is exclusive upper bound
                    JobID = random.Next(1, 51),
                    Exp = (long)(random.NextDouble() * long.MaxValue),
                    Gap = (long)(random.NextDouble() * long.MaxValue),
                    WorldID = random.Next(1, 100),
                    WorldName = worldName,
                    CharacterImgURL = "https://example.com/dummy-image.png"
                });
            }

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(players, Newtonsoft.Json.Formatting.Indented);

            // Save to file
            string fileName = $"fake_players_{timestamp}.json";
            File.WriteAllText(fileName, json);

            Console.WriteLine($"Generated {players.Count} fake players in {fileName}");
        }
    }
}
