namespace MapleTools.Abstraction
{
    public interface ICacheManager
    {
        /// <summary>
        /// All the caches should be cleared with this method
        /// </summary>
        public void ClearAllCache();
        /// <summary>
        /// A single cache based on cache name should be cleared
        /// </summary>
        /// <param name="key"></param>
        public void ClearSingleCache(string name);
        /// <summary>
        /// Get a single cache collection based on name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ICacheEntry? GetCacheByName<T>(string name);

        public void SetCacheByName<T>(string name, ICacheEntry value);

        public List<string> GetAllCacheNames();


    }
}
