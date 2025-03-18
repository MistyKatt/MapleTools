using MapleTools.Abstraction;
using MapleTools.Localization;
using MapleTools.Models.Content;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MapleTools.Util
{
    public class FileDataProvider : IFileAccessor
    {
        private IOptions<LocalizationOptions> _options;
        public FileDataProvider(IOptions<LocalizationOptions> options)
        {
            _options = options;
        }
        public async Task<T> JsonFileReader<T>(string filePath)
        {
            if (File.Exists(filePath) && filePath.EndsWith("json"))
            {
                var version = LatestVersion(filePath);
                //versioned file exists, read latest versioned file instead
                if(!string.IsNullOrEmpty(version))
                {
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    filePath = filePath.Replace(fileName, $"{fileName}_{version}");
                }
                using (StreamReader rs = new StreamReader(filePath))
                {
                    var result = await rs.ReadToEndAsync();
                    var data = JsonConvert.DeserializeObject<T>(result) ?? default(T);
                    return data;
                }
            }
            return default(T);
        }

        public async Task JsonFileWriter<T>(string rootPath, T model)
        {
            var firstFile = Directory.GetFiles(rootPath).FirstOrDefault();
            if (firstFile != null)
            {
                var latestVersion = LatestVersion(firstFile);
                var language = Path.GetFileNameWithoutExtension(firstFile).Split('_')[0];
                string newFileName;
                if (int.TryParse(latestVersion, out var version))
                {
                    //new file to create
                    newFileName = Path.Join(rootPath, language)+"_"+((version + 1) +".json");
                    
                }
                else
                {
                    newFileName = Path.Join(rootPath, language) + "_1.json";
                }
                using (StreamWriter sw = new StreamWriter(newFileName))
                {
                    var str = JsonConvert.SerializeObject(model);
                    await sw.WriteAsync(str);
                }
            }
        }

        private string LatestVersion(string filePath)
        {
            var fileLanguage = Path.GetFileNameWithoutExtension(filePath);
            var dir = Path.GetDirectoryName(filePath);
            var files = Directory.GetFiles(dir).Where(f=>f.Contains(fileLanguage)&&f!=filePath);
            if(files.Count() == 0)
            {
                return "";
            }
            try
            {
                //get filenames without extesion & in lan_version format. Then return the version with max value.
                var version = files.Select(f => Path.GetFileNameWithoutExtension(f)).Where(f => f.Split('_').Count() == 2).Select(f => int.Parse((f.Split('_')[1]))).Max();
                return "" + version;
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        
    }
}
