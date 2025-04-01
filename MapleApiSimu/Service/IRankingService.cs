using MapleTools.Models.Api;
using System.Threading.Tasks;

namespace MapleTools.Models.Ranking
{
    public interface IRankingService
    {
        public List<Player> GetRankingsAsync(string region, string id, int pageIndex);
    }
}