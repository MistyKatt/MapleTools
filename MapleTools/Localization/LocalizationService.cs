using MapleTools.Abstraction;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace MapleTools.Localization
{
    public class LocalizationService
    {

        private ConcurrentDictionary<string, LocalizationDomain> _localization;

        private string[] _domains;

        private List<string> _languages;

        private string _path;

        private LocalizationOptions _options;

        private IWebHostEnvironment _environment;

        public LocalizationService(IOptions<LocalizationOptions> options, IWebHostEnvironment environment)
        {
            _environment = environment;
            _options = options.Value;
            _languages = _options?.Languages ?? ["en", "zh-CN"];
            _path = _options?.RootPath ?? "Translation";
            _localization = new ConcurrentDictionary<string, LocalizationDomain>();
            var test = Directory.GetDirectories(Path.Combine(_environment.ContentRootPath, _path));
            _domains = Directory.GetDirectories(Path.Combine(_environment.ContentRootPath, _path)).Select(d => d.Split('\\').Last()).ToArray();
        }
        /// <summary>
        /// Get the translation value based on the domain, language and key.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="language"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Translation(string domain, string language, string key)
        {
            //Add the situations where the translation should not exist.
            if (domain == null || !_domains.Contains(domain)||language == null||!_languages.Contains(language))
                return key;
            _localization.TryGetValue(domain, out var result);
            //should build it up
            if (result == null)
            {
                result = BuildUpDomain(domain);
                _localization.TryAdd(domain, result);
            }
            return result.Translate(language, key);
        }

        public LocalizationDomain BuildUpDomain(string domain)
        {
            var localizationDomain = new LocalizationDomain();
            var rootPath = Path.Combine(_environment.ContentRootPath, _path, domain);
            foreach(var language in _languages)
            {
                var filePath = Path.Combine(rootPath, language);
                var file = filePath + ".json";
                if (file != null)
                {
                    using (var streamReader = new StreamReader(file))
                    {
                        var content = streamReader.ReadToEnd();
                        var keyValuePair = JsonConvert.DeserializeObject<ConcurrentDictionary<string, string>>(content);
                        localizationDomain.AddLanguage(language, keyValuePair??new ConcurrentDictionary<string, string>());
                    }
                }
            }
            return localizationDomain;
        }
    }
}
