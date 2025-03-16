namespace MapleTools.Abstraction
{
    public interface IFileAccessor
    {
        public Task<T> JsonFileReader<T>(string filePath);

    }
}
