using MapleTools.Localization;

namespace MapleTools.Abstraction
{
    /// <summary>
    /// Getting data from file system
    /// </summary>
    public class FileDataService : IDataService
    {
        
        public FileDataService()
        {

        }
        public virtual Task Aggregate()
        {
            return Task.CompletedTask;
        }
    }
}
