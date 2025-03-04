using MapleTools.Abstraction;

namespace MapleTools.Services.Cache
{
    public class CacheManager : ICacheManager
    {
        private Dictionary<string, ICacheEntry> _cache = new Dictionary<string, ICacheEntry>();
        public void ClearAllCache()
        {
            _cache.Clear();
        }

        public void ClearSingleCache(string name)
        {
            if(_cache.ContainsKey(name))
            {
                _cache.Remove(name);
            }
        }

        public List<string> GetAllCacheNames()
        {
            return _cache.Keys.ToList();
        }

        public ICacheEntry? GetCacheByName<T>(string name)
        {
            if(_cache.ContainsKey(name))
            {
                var cacheEntry = _cache[name];
                if(typeof(T) == cacheEntry.ValueType)
                    return cacheEntry as CacheEntry<T>;
            }
            return null;
        }

        public void SetCacheByName<T>(string name, ICacheEntry value)
        {
            if (!_cache.ContainsKey(name))
            {
                _cache.Add(name, value);
            }
        }
    }

}
