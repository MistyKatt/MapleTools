namespace MapleTools.Abstraction
{
    public interface ICacheEntry
    {
        Type ValueType { get; }
        public void Clear();

    }
}
