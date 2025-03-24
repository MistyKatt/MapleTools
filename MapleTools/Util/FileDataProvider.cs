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
        public async Task<T> JsonFileReader<T>(string filePath, string language)
        {
            if (File.Exists(filePath))
            {
                var version = LatestVersion(filePath, language);
                //versioned file exists, read latest versioned file instead
                if (!string.IsNullOrEmpty(version))
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
                string newFileName;
                if (int.TryParse(latestVersion, out var version))
                {
                    newFileName = Path.Join(rootPath, language) + "_" + ((version + 1) + ".json");
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
            else
            {
                var path = Path.Combine(rootPath, $"{language}.json");
                using (StreamWriter sw = new StreamWriter(path))
                {
                    var str = JsonConvert.SerializeObject(model);
                    await sw.WriteAsync(str);
                }

            }
        }

        private string LatestVersion(string filePath, string language)
        {

            var files = Directory.GetFiles(filePath).Where(f => Path.GetFileNameWithoutExtension(f).Contains(language));
            if (files.Count() <= 1)
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
    }
}
