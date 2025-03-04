﻿using MapleTools.Abstraction;
using MapleTools.Models;
using MapleTools.Services.Cache;
using MapleTools.Simulation;
using System.Runtime.InteropServices;

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
            if (_aggregated.Count==0)
            {
                var cached = _cacheManager.GetCacheByName<List<Player>>(_name);
                if (cached != null)
                {
                    _aggregated = cached as Dictionary<string, List<Player>>?? new Dictionary<string, List<Player>>();
                }
                else
                {
                    var players  = DummyData.Players;
                    _aggregated = players
                        .GroupBy(p => p.JobID)
                        .ToDictionary(
                            g => g.Key.ToString(),
                            g => g.OrderBy(p => p.Gap).Take(10).ToList()
                        );
                    ICacheEntry result = new CacheEntry<List<Player>>(_aggregated);
                    _cacheManager.SetCacheByName<List<Player>>(_name, result);
                }

            }
        }

    }
}
