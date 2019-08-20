//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ease.Util.Temp
{
    /// <summary>
    /// Abstraction of a manager of temporary file objects. All managed temporary file objects will be deleted / released
    /// on `Dispose()` of the manager (provided no locking or other exceptions prevent such deletion -- the cleanup
    /// is best-effort).
    /// </summary>
    public interface IScopedTempFileManager : IDisposable
    {
        /// <summary>
        /// Allocate a new <see cref="IScopedTempFile"/>.
        /// </summary>
        /// <returns></returns>
        IScopedTempFile New();
    }

    /// <summary>
    /// Base class for <see cref="IScopedTempFileManager"/> implementations.
    /// </summary>
    public abstract class ScopedTempFileManager : SafeDisposableWithFinalizer, IScopedTempFileManager
    {
        private readonly HashSet<ScopedTempFile> _files = new HashSet<ScopedTempFile>();

        public IScopedTempFile New()
        {
            var file = AllocateFile();
            _files.Add(file);
            file.AddDisposeAction(() => _files.Remove(file));
            return file;
        }

        protected override void DisposeUnmanagedObjects()
        {
            // Need the .ToList() to avoid enumerating over collection being modified during enumeration
            foreach(var file in _files.ToList())
            {
                file.Dispose();
            }
        }

        protected override void NullifyLargeFields()
        {
            base.NullifyLargeFields();
            _files.Clear();
        }

        /// <summary>
        /// Child classes should allocate a new <see cref="ScopedTempFile"/> and return it here. The base manager
        /// will take care of tracking from this point.
        /// </summary>
        /// <returns></returns>
        protected abstract ScopedTempFile AllocateFile();
    }
}
