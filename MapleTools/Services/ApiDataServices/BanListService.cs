using MapleTools.Abstraction;
using MapleTools.Models.Api;
using MapleTools.Services.Cache;
using MapleTools.Simulation;
using Microsoft.Extensions.Options;

namespace MapleTools.Services.ApiDataServices
{
    /// <summary>
    /// Take List<player> as input and generate job:List<player> dictionary as result
    /// </summary>
    public class BanListService : ApiDataService<Dictionary<string, List<Player>>>
    {


        private readonly string _name = "BanList";

        private string _endpoint;

        public BanListService(IOptions<ServiceOptions> serviceOptions, string name):base(name)
        {
            Data = new Dictionary<string, List<Player>>();
            _endpoint = serviceOptions.Value?.BanListService??"dummy";
        }

        public async override Task Aggregate()
        {
            if (Data.Count == 0)
            {
                var players = DummyData.BannedPlayers;
                Data = players
                    .GroupBy(p => p.JobID)
                    .ToDictionary(
                        g => g.Key.ToString(),
                        g => g.OrderByDescending(p => p.Level).ToList()
                    );
            }
        }
    }
}
