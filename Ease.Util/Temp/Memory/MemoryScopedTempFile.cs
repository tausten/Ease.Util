//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
        private readonly MemoryStream _file = new MemoryStream();

        public override Stream OpenAppend()
        {
            throw new NotImplementedException();
        }

        public override Stream OpenRead()
        {
            throw new NotImplementedException();
        }

        public override Stream OpenWrite()
        {
            throw new NotImplementedException();
        }

        protected override void DisposeManagedObjects()
        {
            base.DisposeManagedObjects();
            _file.Dispose();
        }
    }
}
