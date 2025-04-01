using MapleTools.Models.Api;
using MapleTools.Models.Ranking;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace MapleApiSimu.Service
{
    public class RankingService : IRankingService
    {
        
        private RankingResponse _response;

        private IWebHostEnvironment _environment;

        public RankingService(IWebHostEnvironment webHostEnvironment)
        {
                _response = new RankingResponse();
                _response.TrendingsNA = new List<Player>();
                _response.TrendingsEU = new List<Player>();
                _response.RanksNA = new List<Player>();
                _response.RanksEU = new List<Player>();
                _environment = webHostEnvironment;
        }
        public List<Player> GetRankingsAsync(string region, string id, int pageIndex)
        {
            
            var result = Results(region, id, pageIndex);
            if (result.Count > 0)
                return result;


            var NAPath = Path.Combine(_environment.ContentRootPath, "Data\\Legendary\\na.json");
            var EUPath = Path.Combine(_environment.ContentRootPath, "Data\\Legendary\\eu.json");
            var NATPath = Path.Combine(_environment.ContentRootPath, "Data\\Gandi\\natrend.json");
            var EUTPath = Path.Combine(_environment.ContentRootPath, "Data\\Gandi\\eutrend.json");

            using(var sr = new StreamReader(NAPath))
            {
                var data = sr.ReadToEnd();
                var players = JsonConvert.DeserializeObject<List<Player>>(data);
                _response.RanksNA.AddRange(players);
            }
            using (var sr = new StreamReader(EUPath))
            {
                var data = sr.ReadToEnd();
                var players = JsonConvert.DeserializeObject<List<Player>>(data);
                _response.RanksEU.AddRange(players);
            }

            _response.RanksNA =  _response.RanksNA.OrderByDescending(p => p.Level).ThenByDescending(p => p.Exp).ToList();
            _response.RanksEU = _response.RanksEU.OrderByDescending(p => p.Level).ThenByDescending(p => p.Exp).ToList();
            using (var sr = new StreamReader(NATPath))
            {
                var data = sr.ReadToEnd();
                var players = JsonConvert.DeserializeObject<List<Player>>(data);
                _response.TrendingsNA.AddRange(players);
            }
            using (var sr = new StreamReader(EUTPath))
            {
                var data = sr.ReadToEnd();
                var players = JsonConvert.DeserializeObject<List<Player>>(data);
                _response.TrendingsEU.AddRange(players);
            }

            _response.TrendingsNA = _response.TrendingsNA.OrderByDescending(p=>p.Gap).ToList();
            _response.TrendingsEU = _response.TrendingsNA.OrderByDescending(p => p.Gap).ToList();

            return Results(region, id, pageIndex);
        }

        private int EndIndex(int start, int count)
        {
            int pageEnd = (start + 10 >= count) ? count - start : 10;
            return pageEnd;
        }

        private List<Player> Results(string region, string id, int pageIndex)
        {
            if (id == "weekly")
            {
                if (region == "na" && _response.TrendingsNA.Count > 0)
                {
                    return _response.TrendingsNA.Slice(pageIndex, EndIndex(pageIndex, _response.TrendingsNA.Count));
                }
                else if (region == "eu" && _response.TrendingsEU.Count > 0)
                    return _response.TrendingsEU.Slice(pageIndex, EndIndex(pageIndex, _response.TrendingsEU.Count));
            }
            else if (id == "legendary")
            {
                if (region == "na" && _response.RanksNA.Count > 0)
                {
                    return _response.RanksNA.Slice(pageIndex, EndIndex(pageIndex, _response.RanksNA.Count));
                }
                else if (region == "eu" && _response.RanksEU.Count > 0)
                    return _response.RanksEU.Slice(pageIndex, EndIndex(pageIndex, _response.RanksEU.Count));
            }
            return new List<Player>();
        }
    }
}
