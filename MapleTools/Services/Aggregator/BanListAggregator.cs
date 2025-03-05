using MapleTools.Abstraction;
using MapleTools.Models;
using MapleTools.Services.Cache;
using MapleTools.Simulation;

namespace MapleTools.Services.Aggregator
{
    /// <summary>
    /// Take List<player> as input and generate job:List<player> dictionary as result
    /// </summary>
    public class BanListAggregator : IDataAggregator
    {
        private Dictionary<string, List<Player>> _aggregated;

        private ICacheManager _cacheManager;

        private readonly string _name = "BanList";

        public BanListAggregator(ICacheManager cacheManager)
        {
            _aggregated = new Dictionary<string, List<Player>>();
            _cacheManager = cacheManager;
        }

        public Dictionary<string, List<Player>> Aggregated { get { return _aggregated; } }
        public void Aggregate()
        {
            if (_aggregated.Count == 0)
            {
                var players = DummyData.BannedPlayers;
                _aggregated = players
                    .GroupBy(p => p.JobID)
                    .ToDictionary(
                        g => g.Key.ToString(),
                        g => g.OrderByDescending(p => p.Level).ToList()
                    );
            }
        }
    }
}
