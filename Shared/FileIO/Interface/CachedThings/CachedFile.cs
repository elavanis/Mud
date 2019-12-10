﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shared.FileIO.CachedThings
{
    internal class CachedFile
    {
        internal string FileName { get; set; }
        internal bool Flushed { get; set; }
        internal DateTime LastAccessed { get; set; }
        internal MemoryStream MemoryStream { get; set; }
        internal bool Permanent { get; set; }

        internal CachedFile(string fileName, bool permanent)
        {
            FileName = fileName;
            Permanent = permanent;
            Flushed = true;
        }
    }
}
