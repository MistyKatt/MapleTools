namespace MapleTools.Abstraction
{
    public interface ILocalizationManager
    {
        /// <summary>
        /// Get supported languagues
        /// </summary>
        /// <returns></returns>
        public List<string> GetLanguages();
        /// <summary>
        /// Get the translation dictionary based on language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetTranslationByLanguage(string language);
        /// <summary>
        /// Get the shared translation in case nothing can be found
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetFallbackTranslation();

        public void Initialize(string rootPath);

    }
}
