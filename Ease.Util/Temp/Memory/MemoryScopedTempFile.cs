//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System.IO;

namespace Ease.Util.Temp.Memory
{
    public class MemoryScopedTempFileManager : ScopedTempFileManager
    {
        protected override ScopedTempFile AllocateFile()
        {
            return new MemoryScopedTempFile();
        }
    }

    public class MemoryScopedTempFile : ScopedTempFile
    {
        private class DeferredDisposeMemoryStream : MemoryStream
        {
            private bool _isDisposing = true;
            protected override void Dispose(bool disposing)
            {
                _isDisposing = disposing;
                // Intentionally do nothing at this point
            }

            public void DeferredDispose()
            {
                base.Dispose(_isDisposing);
            }
        }
        private readonly DeferredDisposeMemoryStream _file = new DeferredDisposeMemoryStream();

        public override Stream OpenAppend()
        {
            _file.Seek(0, SeekOrigin.End);
            return _file;
        }

        public override Stream OpenRead()
        {
            _file.Seek(0, SeekOrigin.Begin);
            return _file;
        }

        public override Stream OpenWrite()
        {
            _file.SetLength(0);
            return _file;
        }

        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();
            _file.DeferredDispose();
        }
    }
}
