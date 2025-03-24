namespace MapleTools.Abstraction
{
    public interface IFileAccessor
    {
        /// <summary>
        /// Given the root path and language, read the latest version T model data 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Task<T> JsonFileReader<T>(string filePath, string language);

        /// <summary>
        /// Given the T model, root path and language, write the data to the latest version. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task JsonFileWriter<T>(string filePath, string language, T model);

    }
}
