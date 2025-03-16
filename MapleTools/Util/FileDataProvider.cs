using MapleTools.Abstraction;
using MapleTools.Models.Content;
using Newtonsoft.Json;

namespace MapleTools.Util
{
    public class FileDataProvider : IFileAccessor
    {
        
        public async Task<T> JsonFileReader<T>(string filePath)
        {
            if (File.Exists(filePath) && filePath.EndsWith("json"))
            {
                using (StreamReader rs = new StreamReader(filePath))
                {
                    var result = await rs.ReadToEndAsync();
                    var data = JsonConvert.DeserializeObject<T>(result) ?? default(T);
                    return data;
                }
            }
            return default(T);
        }
    }
}
