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
    public class FarmingService : ApiDataService<Dictionary<string, List<Player>>>
    {

        private readonly string _name = "Farming";

        private string _endpoint;

        public FarmingService(IOptions<ServiceOptions> serviceOptions):base()
        {
            Data = new Dictionary<string, List<Player>>();
            _endpoint = serviceOptions.Value?.FarmingService??"dummy";
        }
        public async override Task Aggregate()
        {
            if (Data.Count == 0)
            {
                var players = DummyData.FarmingPlayers;
                Data.Add("Kronos", players.OrderByDescending(p=>p.Gap).ToList());                  
            }
        }

    }
}
