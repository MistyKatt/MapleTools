using MapleTools.Abstraction;
using MapleTools.Localization;
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
        public async Task<T> JsonFileReader<T>(string rootPath, string language)
        {
            if (Directory.Exists(rootPath))
            {
                var version = LatestVersion(rootPath, language);
                var filePath = FileNameWithVersion(rootPath, language, version);
                if (File.Exists(filePath))
                {
                    using (StreamReader rs = new StreamReader(filePath))
                    {
                        var result = await rs.ReadToEndAsync();
                        var data = JsonConvert.DeserializeObject<T>(result) ?? default(T);
                        return data;
                    }
                }
            }
            return default(T);
        }
        /// <summary>
        /// Given a rootpath (folder), save the model data under the folder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootPath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task JsonFileWriter<T>(string rootPath, string language, T model)
        {
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            var filesInLanguage = Directory.GetFiles(rootPath).Where(f => Path.GetFileNameWithoutExtension(f).Contains(language));
            if (filesInLanguage.Count() > 0)
            {
                var latestVersion = LatestVersion(rootPath, language);
                var newFileName = CreateFileNameWithVersion(rootPath, language, latestVersion);
                using (StreamWriter sw = new StreamWriter(newFileName))
                {
                    var str = JsonConvert.SerializeObject(model);
                    await sw.WriteAsync(str);
                }
            }
            //initialize base file for each language
            else
            {
                foreach (var lang in _options.Value.Languages)
                {
                    var path = Path.Combine(rootPath, $"{lang}.json");
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        var str = JsonConvert.SerializeObject(model);
                        await sw.WriteAsync(str);
                    }
                }

            }
        }

        private string LatestVersion(string filePath, string language)
        {

            var files = Directory.GetFiles(filePath).Where(f => Path.GetFileNameWithoutExtension(f).Contains(language)).Where(f=>Path.GetFileNameWithoutExtension(f).Split('_').Count() == 2);
            if (files.Count() == 0)
            {
                return "";
            }
            try
            {
                //get filenames without extesion & in lan_version format. Then return the version with max value.
                var version = files.Select(f => Path.GetFileNameWithoutExtension(f)).Where(f => f.Split('_').Count() == 2).Select(f => int.Parse((f.Split('_')[1]))).Max();
                return "" + version;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string FileNameWithVersion(string rootPath, string language, string version)
        {
            var result = "";
            if (int.TryParse(version, out var versionNumber))
            {
                result = Path.Join(rootPath, language) + "_" + ((versionNumber) + ".json");
            }
            else
            {
                result = Path.Join(rootPath, language) + ".json";
            }
            return result;
        }

        private string CreateFileNameWithVersion(string rootPath, string language, string version)
        {
            var result = "";
            if (int.TryParse(version, out var versionNumber))
            {
                result = Path.Join(rootPath, language) + "_" + ((versionNumber+1) + ".json");
            }
            else if(File.Exists(Path.Join(rootPath, language) + ".json"))
            {
                result = Path.Join(rootPath, language) + "_1.json";
            }
            else
                result = Path.Join(rootPath, language) + ".json";
            return result;
        }
    }
}
