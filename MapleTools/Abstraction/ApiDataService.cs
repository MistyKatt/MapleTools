namespace MapleTools.Abstraction
{
    /// <summary>
    /// Fetching the data service with api requests
    /// </summary>
    public class ApiDataService<T> : IDataService<T>
    {

        public ApiDataService(string name)
        {
            ServiceName = name;
        }

        public T Data { get ; set ; }
        public string ServiceName { get; set; }

        public virtual Task Aggregate()
        {
            return Task.CompletedTask;
        }
    }
}
