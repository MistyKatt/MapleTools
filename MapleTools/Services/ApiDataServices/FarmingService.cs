using MapleTools.Abstraction;
using MapleTools.Models;
using MapleTools.Simulation;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace MapleTools.Services.ApiDataServices
{
    /// <summary>
    /// Take List<player> as input and return job:List<player> as output
    /// </summary>
    public class FarmingService : ApiDataService
    {
        private Dictionary<string, List<Player>> _aggregated;

        private readonly string _name = "Farming";

        private string _endpoint;

        public FarmingService(IOptions<ServiceOptions> serviceOptions):base()
        {
            _aggregated = new Dictionary<string, List<Player>>();
            _endpoint = serviceOptions.Value?.FarmingService??"dummy";
        }

        public Dictionary<string, List<Player>> Aggregated { get { return _aggregated; } }
        public async override Task Aggregate()
        {
            if (_aggregated.Count == 0)
            {
                var players = DummyData.FarmingPlayers;
                _aggregated.Add("Kronos", players.OrderByDescending(p=>p.Gap).ToList());                  
            }
        }

    }
}
