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
    /// Abstraction of a manager of temporary files. All managed temporary file objects will be deleted / released
    /// on `Dispose()` of the manager (provided no locking or other exceptions prevent such deletion -- the cleanup
    /// is best-effort). Sample usage:
    /// 
    /// <code>
    /// IScopedTempFileManager manager; // Concrete implementation obtained somehow
    /// using (manager)
    /// {
    ///     var first = manager.New();
    ///     using (var writer = new StreamWriter(first.OpenWrite()))
    ///     {
    ///         writer.Write("Stuff to store in temp file");
    ///     }
    ///     // ... things happen
    ///     // Maybe we need to read the file back now...
    ///     using (var reader = new StreamReader(first.OpenRead())
    ///     {
    ///         var contents = reader.ReadToEnd();
    ///     }
    ///     // ... maybe we need another different temp file...
    ///     var second = manager.New();
    ///     // ... do stuff with the other file...
    /// }
    /// // ... at this point, all temp files are cleaned up
    /// </code>
    /// 
    /// That same logic applies whether the temp files are backed by an in-memory store, local filesystem, 
    /// or any other concrete implementation (eg. AWS S3, Azure storage, DropBox, etc...). Default implementations
    /// for in-memory (<see cref="Memory.MemoryScopedTempFileManager"/>) and local filesystem
    /// (<see cref="Local.LocalScopedTempFileManager"/>) are in this package. Other concrete implementations 
    /// may be implemented in other packages to avoid introducing excessive dependencies.
    /// </summary>
    public interface IScopedTempFileManager : IDisposable
    {
        /// <summary>
        /// Allocate a new <see cref="IScopedTempFile"/>.
        /// </summary>
        /// <returns>The newly allocated <see cref="IScopedTempFile"/></returns>
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
