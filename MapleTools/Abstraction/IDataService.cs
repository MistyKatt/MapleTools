namespace MapleTools.Abstraction
{
    /// <summary>
    /// Aggregate data from various datasource
    /// </summary>
    public interface IDataService<T>
    {
        public T Data {  get; set; }

        public string ServiceName { get; set; }
        public Task Aggregate();
    }
}
