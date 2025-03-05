using MapleTools.Abstraction;
using MapleTools.Simulation;

namespace MapleTools.Services.Aggregator
{
    /// <summary>
    /// Take List<player> as input and generate level: (job, count) as output 
    /// </summary>
    public class TrendingAggregator : IDataAggregator
    {
        private Dictionary<string, List<(string, int)>> _aggregated;

        private ICacheManager _cacheManager;

        private readonly string _name = "Trending";

        public TrendingAggregator(ICacheManager cacheManager)
        {
            _aggregated = new Dictionary<string, List<(string, int)>>();
            _cacheManager = cacheManager;
        }

        public Dictionary<string, List<(string, int)>> Aggregated { get { return _aggregated; } }
        public void Aggregate()
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
