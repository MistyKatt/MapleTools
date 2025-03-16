namespace MapleTools.Abstraction
{
    /// <summary>
    /// Fetching the data service with api requests
    /// </summary>
    public class ApiDataService<T> : IDataService<T>
    {

        public ApiDataService()
        {

        }

        public T Data { get ; set ; }

        public virtual Task Aggregate()
        {
            return Task.CompletedTask;
        }
    }
}
