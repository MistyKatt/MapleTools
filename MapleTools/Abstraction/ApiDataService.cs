namespace MapleTools.Abstraction
{
    /// <summary>
    /// Fetching the data service with api requests
    /// </summary>
    public class ApiDataService : IDataService
    {

        public ApiDataService()
        {

        }
        public virtual Task Aggregate()
        {
            return Task.CompletedTask;
        }
    }
}
