using Objects.Global.FileIO.Interface;
using Shared.FileIO.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Objects.Global.FileIO
{
    public class CachedFileIO : ICachedFileIO
    {
        private object PadLock = new object();
        private Dictionary<string, CachedFile> CachedFiles = new Dictionary<string, CachedFile>();
        private List<string> PermanentCachedDirectories = null;
        private IFileIO FileIO;

        public CachedFileIO(List<string> permanentCachedDirectories, IFileIO fileIO)
        {
            FileIO = fileIO;

            if (permanentCachedDirectories == null)
            {
                permanentCachedDirectories = new List<string>();
            }

            PermanentCachedDirectories = permanentCachedDirectories;

            LoadPermanentFilesIntoMemory();
        }

        private void LoadPermanentFilesIntoMemory()
        {
            foreach (string directory in PermanentCachedDirectories)
            {
                GetFilesFromDirectoryWhileLoadingIntoMemory(directory);
            }
        }

        #region Write
        public void AppendFile(string fileName, IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                AppendFile(fileName, line);
            }
        }

        public void AppendFile(string fileName, string line)
        {
            AppendStream(fileName, Encoding.UTF8.GetBytes(line + Environment.NewLine));
        }

        public void WriteFile(string fileName, string file)
        {
            WriteStream(fileName, Encoding.UTF8.GetBytes(file + Environment.NewLine));
        }

        public void WriteFile(string fileName, byte[] bytes)
        {
            WriteStream(fileName, bytes);
        }

        public void WriteFileBase64(string fileName, string file)
        {
            byte[] bytes = Convert.FromBase64String(file + Environment.NewLine);
            WriteStream(fileName, bytes);
        }
        #endregion Write

        #region Read
        public string ReadAllText(string fileName)
        {
            return Encoding.UTF8.GetString(ReadStream(fileName));
        }

        public byte[] ReadBytes(string fileName)
        {
            return ReadStream(fileName);
        }

        public string ReadFileBase64(string fileName)
        {
            return Convert.ToBase64String(ReadStream(fileName));
        }

        public string[] ReadLines(string fileName)
        {
            return ReadAllText(fileName).Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }
        #endregion Read

        #region Other
        public string[] GetFilesFromDirectory(string directory)
        {
            lock (PadLock)
            {
                string[] files = CachedFiles.Keys.Where(e => e.StartsWith(directory)).ToArray();
                return files;
            }
        }

        private string[] GetFilesFromDirectoryWhileLoadingIntoMemory(string directory)
        {
            string[] files = FileIO.GetFilesFromDirectory(directory);

            //ensure each file is in the cache
            foreach (string file in files)
            {
                ReadStream(file);
            }

            return files;
        }

        public bool Exists(string fileName)
        {
            lock (PadLock)
            {
                return CachedFiles.ContainsKey(fileName);
            }
        }

        public void Flush()
        {
            try
            {
                lock (PadLock)
                {
                    string[] keys = CachedFiles.Keys.ToArray();

                    foreach (string key in keys)
                    {
                        CachedFile cachedFile = CachedFiles[key];

                        if (!cachedFile.Flushed)
                        {
                            FileIO.WriteFile(cachedFile.FileName, cachedFile.MemoryStream.ToArray());
                            cachedFile.Flushed = true;
                        }

                        if (!cachedFile.Permanent
                            && DateTime.Now.Subtract(cachedFile.LastAccessed).TotalHours > 24)
                        {
                            CachedFiles.Remove(key);
                        }
                    }
                }
            }
            catch
            {
                //file system is down
                //wait till the next flush
            }
        }

        public void ReloadCache()
        {
            lock (PadLock)
            {
                Flush();
                CachedFiles = new Dictionary<string, CachedFile>();
                {
                    LoadPermanentFilesIntoMemory();
                }
            }
        }

        public void Delete(string fileName)
        {
            FileIO.Delete(fileName);
        }
        #endregion Other

        private CachedFile GetStream(string fileName)
        {
            CachedFile cachedFile = null;
            lock (PadLock)
            {
                CachedFiles.TryGetValue(fileName, out cachedFile);

                if (cachedFile == null)
                {
                    cachedFile = new CachedFile(fileName, IsPermanent(fileName));

                    CachedFiles.Add(fileName, cachedFile);
                }
            }

            return cachedFile;
        }

        private MemoryStream LoadFromFile(string fileName)
        {
            MemoryStream memoryStream = new MemoryStream();
            if (FileIO.Exists(fileName))
            {
                byte[] bytes = FileIO.ReadBytes(fileName);
                memoryStream.Write(bytes, 0, bytes.Length);
            }

            return memoryStream;
        }

        private byte[] ReadStream(string fileName)
        {
            CachedFile cachedFile = GetStream(fileName);
            lock (cachedFile)
            {
                if (cachedFile.MemoryStream == null)
                {
                    cachedFile.MemoryStream = LoadFromFile(fileName);
                }

                cachedFile.LastAccessed = DateTime.Now;

                return cachedFile.MemoryStream.ToArray();
            }
        }

        private void AppendStream(string fileName, byte[] dataToWrite)
        {
            CachedFile cachedFile = GetStream(fileName);
            lock (cachedFile)
            {
                if (cachedFile.MemoryStream == null)
                {
                    cachedFile.MemoryStream = LoadFromFile(fileName);
                }

                AppendStream(cachedFile, dataToWrite);
            }
        }

        private void WriteStream(string fileName, byte[] dataToWrite)
        {
            CachedFile cachedFile = GetStream(fileName);
            lock (cachedFile)
            {
                cachedFile.MemoryStream = new MemoryStream();
                AppendStream(cachedFile, dataToWrite);
            }
        }

        private void AppendStream(CachedFile cachedFile, byte[] dataToWrite)
        {
            //we are already locked from the calling method
            cachedFile.LastAccessed = DateTime.Now;
            cachedFile.Flushed = false;
            cachedFile.MemoryStream.Seek(0, SeekOrigin.End);
            cachedFile.MemoryStream.Write(dataToWrite, 0, dataToWrite.Length);
        }

        private bool IsPermanent(string fileName)
        {
            foreach (string directory in PermanentCachedDirectories)
            {
                if (fileName.StartsWith(directory))
                {
                    return true;
                }
            }

            return false;
        }


    }
}
