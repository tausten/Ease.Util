//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System.IO;

namespace Ease.Util.Temp.Memory
{
    public class LocalScopedTempFileManager : ScopedTempFileManager
    {
        private readonly DirectoryInfo _directoryInfo;

        public LocalScopedTempFileManager() : this(null) { }

        public LocalScopedTempFileManager(DirectoryInfo directoryInfo)
        {
                _directoryInfo = directoryInfo;
        }

        protected override ScopedTempFile AllocateFile()
        {
            var newFileInfo = SafeGetNewFileInfo();
            return new LocalScopedTempFile(newFileInfo);
        }

        private FileInfo SafeGetNewFileInfo()
        {
            FileInfo result = null;
            if (null != _directoryInfo)
            {
                while (null == result)
                {
                    // Ensure the full path to the directory exists before trying to create files under it...
                    Directory.CreateDirectory(_directoryInfo.FullName);

                    try
                    {
                        var candidate = Path.Combine(_directoryInfo.FullName, Path.GetRandomFileName());
                        // Intentionally using an explicit exists check the further narrow the race window 
                        // before attempting the actual create.
                        if (!File.Exists(candidate))
                        {
                            using (var fs = new FileStream(candidate, FileMode.CreateNew))
                            {
                                fs.Flush();
                            }
                            result = new FileInfo(candidate);
                        }
                    }
                    catch (IOException)
                    {
                        // Try again with different file.
                    }
                }
            }
            else
            {
                result = new FileInfo(Path.GetTempFileName());
            }
            return result;
        }

        protected override void DisposeUnmanagedObjects()
        {
            base.DisposeUnmanagedObjects();
            if (null != _directoryInfo)
            {
                _directoryInfo.Delete(recursive: true);
            }
        }
    }

    public class LocalScopedTempFile : ScopedTempFile
    {
        private readonly FileInfo _fileInfo;

        public LocalScopedTempFile(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public override Stream OpenAppend()
        {
            return _fileInfo.Open(FileMode.Append);
        }

        public override Stream OpenRead()
        {
            return _fileInfo.OpenRead();
        }

        public override Stream OpenWrite()
        {
            return _fileInfo.OpenWrite();
        }

        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();
            _fileInfo.Delete();
        }
    }
}
