using MapleTools.Abstraction;
using MapleTools.Models;
using MapleTools.Simulation;

namespace MapleTools.Services.Aggregator
{
    /// <summary>
    /// Take List<player> as input and return job:List<player> as output
    /// </summary>
    public class FarmingAggregator : IDataAggregator
    {
        private Dictionary<string, List<Player>> _aggregated;

        private ICacheManager _cacheManager;

        private readonly string _name = "Farming";

        public FarmingAggregator(ICacheManager cacheManager)
        {
            _aggregated = new Dictionary<string, List<Player>>();
            _cacheManager = cacheManager;
        }

        public Dictionary<string, List<Player>> Aggregated { get { return _aggregated; } }
        public void Aggregate()
        {
            if (_aggregated.Count == 0)
            {
                var players = DummyData.FarmingPlayers;
                _aggregated.Add("Kronos", players.OrderByDescending(p=>p.Gap).ToList());                  
            }
        }

    }
}
