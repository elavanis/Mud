using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Objects.Global.FileIO
{
    public class CachedFile
    {
        public string FileName { get; set; }
        public bool Flushed { get; set; }
        public DateTime LastAccessed { get; set; }
        public MemoryStream MemoryStream { get; set; }
        public bool Permanent { get; set; }

        public CachedFile(string fileName, bool permanent)
        {
            FileName = fileName;
            Permanent = permanent;
            Flushed = true;
        }
    }
}
