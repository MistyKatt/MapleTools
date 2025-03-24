using MapleTools.Localization;
using MapleTools.Models.Content;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace MapleTools.Abstraction
{
    /// <summary>
    /// Getting data from file system
    /// </summary>
    public class FileDataService<T> : IDataService<T>
    {
        
        public FileDataService(IFileAccessor fileAccessor, string name)
        {
            FileAccessor = fileAccessor;
            Languages = new List<string>();
            ServiceName = name;
        }

        public IFileAccessor FileAccessor;
        //Each language will have one file path
        public string FilePath { get; set; }
        public List<string> Languages { get; set; }

        public T Data { get ; set ; }
        public string ServiceName { get; set; }

        public virtual Task Aggregate()
        {
            return Task.CompletedTask;
        }

        public virtual void Clear() { }

    }
}
