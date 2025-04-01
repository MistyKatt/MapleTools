using MapleTools.Util;

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
        public Task<T> JsonFileReader<T>(string rootPath, string language, int version);

        /// <summary>
        /// Given the T model, root path and language, write the data to the latest version. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task JsonFileWriter<T>(string rootPath, string language, SaveMode mode, T model);

    }
}
