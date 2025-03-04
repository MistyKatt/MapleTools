using MapleTools.Abstraction;

namespace MapleTools.Services.Localization
{
    public class LocalizationManager : ILocalizationManager
    {
        
        private Dictionary<string, Dictionary<string , string>> _localization;

        public LocalizationManager()
        {
            _localization = new Dictionary<string, Dictionary<string, string>>();
        }
        public Dictionary<string, string> GetFallbackTranslation()
        {
            throw new NotImplementedException();
        }

        public List<string> GetLanguages()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetTranslationByLanguage(string language)
        {
            throw new NotImplementedException();
        }



        public void Initialize(string filePath)
        {
            
        }
    }
}
