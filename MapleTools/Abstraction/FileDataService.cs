using MapleTools.Localization;
using Microsoft.Extensions.Options;

namespace MapleTools.Abstraction
{
    /// <summary>
    /// Getting data from file system
    /// </summary>
    public class FileDataService<T> : IDataService<T>
    {
        
        public FileDataService(IFileAccessor fileAccessor, IOptions<LocalizationOptions> options)
        {
            FileAccessor = fileAccessor;
            FilePath = new Dictionary<string, string>();
            Languages = options.Value.Languages;
        }

        public IFileAccessor FileAccessor;
        //Each language will have one file path
        public Dictionary<string, string> FilePath { get; set; }

        public List<string> Languages { get; set; }

        public T Data { get ; set ; }

        public virtual Task Aggregate()
        {
            return Task.CompletedTask;
        }
    }
}
