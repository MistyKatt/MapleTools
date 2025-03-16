using MapleTools.Abstraction;
using MapleTools.Simulation;
using Microsoft.Extensions.Options;

namespace MapleTools.Services.ApiDataServices
{
    /// <summary>
    /// Take List<player> as input and generate level: (job, count) as output 
    /// </summary>
    public class TrendingService : ApiDataService<Dictionary<string, List<(string, int)>>>
    {

        private readonly string _name = "Trending";

        private string _endpoint;

        public TrendingService(IOptions<ServiceOptions> serviceOptions)
        {
            Data = new Dictionary<string, List<(string, int)>>();
            _endpoint = serviceOptions.Value?.TrendingService??"dummy";
        }
        public async override Task Aggregate()
        {
            if (Data.Count == 0)
            {
                var players = DummyData.Players;
                var players280 = players.Where(p => p.Level >= 280);
                var players285 = players.Where(p => p.Level >= 285);
                var players290 = players.Where(p => p.Level >= 290);

                Data.Add("280", players280.GroupBy(p => p.JobID).Select(p => (p.Key.ToString(), p.Count())).OrderByDescending(p => p.Item2).ToList());
                Data.Add("285", players285.GroupBy(p => p.JobID).Select(p => (p.Key.ToString(), p.Count())).OrderByDescending(p => p.Item2).ToList());
                Data.Add("290", players290.GroupBy(p => p.JobID).Select(p => (p.Key.ToString(), p.Count())).OrderByDescending(p => p.Item2).ToList());

            }
        }
    }
}
