using System.Collections.Generic;
using static Shared.FileIO.Interface.CachedThings.FileExits;

namespace Shared.FileIO.Interface
{
    public interface ICachedFileIO : IFileIO
    {
        void Flush();
        void ReloadCache();
    }
}