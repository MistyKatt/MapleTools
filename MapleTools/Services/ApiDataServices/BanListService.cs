using MapleTools.Abstraction;
using MapleTools.Models;
using MapleTools.Services.Cache;
using MapleTools.Simulation;
using Microsoft.Extensions.Options;

namespace MapleTools.Services.ApiDataServices
{
    /// <summary>
    /// Take List<player> as input and generate job:List<player> dictionary as result
    /// </summary>
    public class BanListService : ApiDataService
    {
        private Dictionary<string, List<Player>> _aggregated;

        private readonly string _name = "BanList";

        private string _endpoint;

        public BanListService(IOptions<ServiceOptions> serviceOptions):base()
        {
            _aggregated = new Dictionary<string, List<Player>>();
            _endpoint = serviceOptions.Value?.BanListService??"dummy";
        }

        public Dictionary<string, List<Player>> Aggregated { get { return _aggregated; } }
        public async override Task Aggregate()
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
