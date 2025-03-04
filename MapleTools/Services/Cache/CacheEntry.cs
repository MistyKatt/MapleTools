


namespace MapleTools.Services.Cache
{
    public class CacheEntry<T>:MapleTools.Abstraction.ICacheEntry
    {
        private Dictionary<string, T> _cache;

        public Type ValueType => typeof(T);

        public CacheEntry() 
        { 
            _cache = new Dictionary<string, T>();
        }

        public CacheEntry(Dictionary<string, T> cache)
        {
            _cache = cache;
        }

        public T? GetCacheByKey(string key)
        {
            if (_cache.TryGetValue(key, out T value))
                return value;
            return default;
        }

        public void SetCacheByKey(string key, T value)
        {
            if( _cache.ContainsKey(key))
            {
                _cache[key] = value;
            }
            else
            {
                _cache.Add(key, value);
            }
        }

        public void RemoveCacheEntryByKey(string key, T value)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.Remove(key);
            }
        }

        public Dictionary<string, T> ReturnInnerCache() => _cache;

        public void Clear()
        {
            _cache.Clear();
        }
    }
}
