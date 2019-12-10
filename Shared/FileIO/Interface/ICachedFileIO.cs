namespace Shared.FileIO.Interface
{
    public interface ICachedFileIO : IFileIO
    {
        void Flush();
        void ReloadCache();
    }
}