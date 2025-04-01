
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MapleTools.Models.Ranking;
using MapleTools.Models.Api;

namespace MapleTools.Controllers
{
    [ApiController]
    [Route("")]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService _rankingService;

        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        [HttpGet("{region}")]
        public ActionResult<List<Player>> GetRankings(
            [FromRoute] string region,
            [FromQuery] string id="legendary",
            [FromQuery] int page_index = 1)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(region) ||
                (region != "na" && region != "eu"))
            {
                region = "na";
            }

            if (string.IsNullOrEmpty(id) ||
                (id != "legendary" && id != "weekly"))
            {
                id = "legendary";
            }

            if (page_index < 1)
            {
                page_index = 1;
            }

            try
            {
                var result = _rankingService.GetRankingsAsync(region, id, page_index);
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching rankings.");
            }
        }
    }
}