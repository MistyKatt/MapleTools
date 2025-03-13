using System.Collections.Concurrent;

namespace MapleTools.Localization
{
    public class LocalizationDomain
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _translation;

        public LocalizationDomain()
        {
            _translation = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
        }

        public string Translate(string language, string key)
        {
            if (language == null || !_translation.ContainsKey(language))
                return key;
            _translation.TryGetValue(language, out var keyValuePair);
            if (keyValuePair == null||!keyValuePair.ContainsKey(key))
                return key;
            keyValuePair.TryGetValue(key, out var value);
            return value??key;
        }

        public void AddLanguage(string language, ConcurrentDictionary<string, string> keyValuePairs)
        {
            _translation.TryAdd(language, keyValuePairs);
        }
    }
}
