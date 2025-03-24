using System.Collections.Concurrent;

namespace MapleTools.Abstraction
{
    public interface IDetailedContent<T>
    {
        public ConcurrentDictionary<string, ConcurrentDictionary<string, T>> DetailedContent { get; set;}
    }
}
