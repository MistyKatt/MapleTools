namespace MapleTools.Abstraction
{
    public interface IFileAccessor
    {
        public Task<T> JsonFileReader<T>(string filePath);

        public Task JsonFileWriter<T>(string filePath, T model);

    }
}
