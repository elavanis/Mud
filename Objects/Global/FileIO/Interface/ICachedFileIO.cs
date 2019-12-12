using Shared.FileIO.Interface;

namespace Objects.Global.FileIO.Interface
{
    public interface ICachedFileIO : IFileIO
    {
        void Flush();
        void ReloadCache();
    }
}