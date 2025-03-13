using MapleTools.Abstraction;
using MapleTools.Simulation;
using Microsoft.Extensions.Options;

namespace MapleTools.Services.ApiDataServices
{
    /// <summary>
    /// Take List<player> as input and generate level: (job, count) as output 
    /// </summary>
    public class TrendingService : ApiDataService
    {
        private Dictionary<string, List<(string, int)>> _aggregated;

        private readonly string _name = "Trending";

        private string _endpoint;

        public TrendingService(IOptions<ServiceOptions> serviceOptions)
        {
            _aggregated = new Dictionary<string, List<(string, int)>>();
            _endpoint = serviceOptions.Value?.TrendingService??"dummy";
        }

        public Dictionary<string, List<(string, int)>> Aggregated { get { return _aggregated; } }
        public async override Task Aggregate()
        {
            if (_aggregated.Count == 0)
            {
                var players = DummyData.Players;
                var players280 = players.Where(p => p.Level >= 280);
                var players285 = players.Where(p => p.Level >= 285);
                var players290 = players.Where(p => p.Level >= 290);

                _aggregated.Add("280", players280.GroupBy(p => p.JobID).Select(p => (p.Key.ToString(), p.Count())).OrderByDescending(p => p.Item2).ToList());
                _aggregated.Add("285", players285.GroupBy(p => p.JobID).Select(p => (p.Key.ToString(), p.Count())).OrderByDescending(p => p.Item2).ToList());
                _aggregated.Add("290", players290.GroupBy(p => p.JobID).Select(p => (p.Key.ToString(), p.Count())).OrderByDescending(p => p.Item2).ToList());

            }
        }
    }
}
