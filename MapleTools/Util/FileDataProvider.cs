using MapleTools.Abstraction;
using MapleTools.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace MapleTools.Util
{
    public enum SaveMode
    {
        Simple,
        LanguageNoVersion,
        VersionNoLanguage,
        VersionAndLanguage
    }
    public class FileDataProvider : IFileAccessor
    {
        private IOptions<LocalizationOptions> _options;
        public FileDataProvider(IOptions<LocalizationOptions> options)
        {
            _options = options;
        }

        /// <summary>
        /// Read the model data from specific path and based on specific language, version
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootPath">The folder that represent the item data</param>
        /// <param name="language">indicate that the item has multiple languages</param>
        /// <param name="version">indicate that the item has multiple versions, -1 means no version and 9999 means latest version</param>
        /// <returns></returns>
        public async Task<T> JsonFileReader<T>(string rootPath, string language, int version)
        {
            if (Directory.Exists(rootPath))
            {
                if (string.IsNullOrEmpty(language))
                {
                    if (version == -1)
                    {
                        return await SimpleFileReader<T>(rootPath);
                    }
                    return await VersionFileReaderNoLanguage<T>(rootPath, version);
                }
                if (version == -1)
                    return await LanguageFileReaderNoVersion<T>(rootPath, language);

                var filePath = "";
                if (version == 9999)
                {
                    version = LatestVersion(rootPath, language);
                }
                filePath = Path.Combine(rootPath, $"{language}_{version}.json");
                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(rootPath, $"{language}.json");
                }
                return await ModelFromFile<T>(filePath);
            }
            return default(T);
        }
        /// <summary>
        /// Given a rootpath (folder), language, mode, save the model data under the folder
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootPath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task JsonFileWriter<T>(string rootPath, string language, SaveMode mode, T model)
        {
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            var filePath = "";
            var latestVersion = 0;
            switch(mode)
            {
                case SaveMode.Simple:
                    filePath = Path.Combine(rootPath, "data.json");
                    await SimpleFileWriter<T>(rootPath, model);
                    break;
                case SaveMode.LanguageNoVersion:
                    filePath = Path.Combine(rootPath, $"{language}.json");
                    await SimpleFileWriter<T>(rootPath, model);
                    break;
                case SaveMode.VersionNoLanguage:
                    latestVersion = LatestVersion(rootPath, "data");
                    if (latestVersion == -1)
                    {
                        filePath = Path.Combine(rootPath, $"data.json");
                        if (File.Exists(filePath))
                            filePath = Path.Combine(rootPath, $"data_1.json");
                    }
                    else
                        filePath = Path.Combine(rootPath, $"data_{latestVersion + 1}.json");
                    await SimpleFileWriter<T>(rootPath, model);
                    break;
                case SaveMode.VersionAndLanguage:
                    latestVersion = LatestVersion(rootPath, language);
                    if (latestVersion == -1)
                    {
                        filePath = Path.Combine(rootPath, $"{language}.json");
                        if (File.Exists(filePath))
                            filePath = Path.Combine(rootPath, $"{language}_1.json");
                    }
                    filePath = Path.Combine(rootPath, $"{language}_{latestVersion + 1}.json");
                    await SimpleFileWriter<T>(rootPath, model);
                    break;
                default:
                    break;

            }
        }

        private int LatestVersion(string filePath, string language)
        {

            var files = Directory.GetFiles(filePath).Where(f => Path.GetFileNameWithoutExtension(f).Contains(language)).Where(f=>Path.GetFileNameWithoutExtension(f).Split('_').Count() == 2);
            if (files.Count() == 0)
            {
                return -1;
            }
            try
            {
                //get filenames without extesion & in lan_version format. Then return the version with max value.
                var version = files.Select(f => Path.GetFileNameWithoutExtension(f)).Where(f => f.Split('_').Count() == 2).Select(f => int.Parse((f.Split('_')[1]))).Max();
                return version;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// Read data without any language or version
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rootPath">The root path of the item, the file name will be data.json always</param>
        /// <returns></returns>
        private async Task<T> SimpleFileReader<T>(string rootPath)
        {
            var filePath = Path.Combine(rootPath, "data.json");
            if (!File.Exists(filePath))
            {
                return default(T);
            }

            return await ModelFromFile<T>(filePath);
        }

        private async Task<T> VersionFileReaderNoLanguage<T>(string rootPath, int version)
        {
            string filePath = filePath = Path.Combine(rootPath, $"data.json");
            if (!File.Exists(filePath))
            {
                return default(T);
            }
            if (version != 0)
            {
                filePath = Path.Combine(rootPath, $"data_{version}.json");
                if (!File.Exists(filePath))
                {
                    filePath = Path.Combine(rootPath, $"data.json");//fallback
                }
            }
            return await ModelFromFile<T>(filePath);

        }

        private async Task<T> LanguageFileReaderNoVersion<T>(string rootPath, string language)
        {
            string filePath = Path.Combine(rootPath, $"{language}.json");
            if (!File.Exists(filePath))
            {
                return default(T);
            }
            return await ModelFromFile<T>(filePath);
        }

        private async Task<T> ModelFromFile<T>(string filePath)
        {
            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                // Log the exception - replace with your actual logging mechanism
                Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                return default(T);
            }
        }

        private async Task SimpleFileWriter<T>(string filePath, T model)
        {
            var text = JsonConvert.SerializeObject(model);
            await File.WriteAllTextAsync(filePath, text);
        }     
    }
}
