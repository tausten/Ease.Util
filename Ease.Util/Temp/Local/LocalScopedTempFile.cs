//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System.IO;
using Ease.Util.Extensions;

namespace Ease.Util.Temp.Local
{
    /// <summary>
    /// Implementation of <see cref="IScopedTempFileManager"/> backed by local filesystem. This will incur 
    /// local file IO overhead, so be sure that this overhead is acceptable before using.
    /// </summary>
    public class LocalScopedTempFileManager : ScopedTempFileManager
    {
        private readonly DirectoryInfo _directoryInfo;

        /// <summary>
        /// Default temporary file management mechanism will be used.
        /// </summary>
        public LocalScopedTempFileManager() : this(null) { }

        /// <summary>
        /// Temporary files will be managed under the specified <paramref name="directoryInfo"/>. The manager
        /// assumes it takes ownership of the directory (and any subdirectories) and will delete them when
        /// `Dispose(..)` is called, so do not attempt to have multiple managers operating on the same folder
        /// or non-deterministic behavior will occur.
        /// </summary>
        /// <param name="directoryInfo">The DirectoryInfo specifying the directory to manage as a temp folder</param>
        public LocalScopedTempFileManager(DirectoryInfo directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }

        protected override ScopedTempFile AllocateFile()
        {
            var newFileInfo = new FileInfo(null != _directoryInfo
                ? _directoryInfo.GetTempFileName()
                : Path.GetTempFileName());

            return new LocalScopedTempFile(newFileInfo);
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

    /// <summary>
    /// Implementation of <see cref="ScopedTempFile"/> backed by local filesystem files.
    /// </summary>
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
