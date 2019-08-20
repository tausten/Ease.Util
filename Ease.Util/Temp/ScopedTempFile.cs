//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ease.Util.Temp
{
    /// <summary>
    /// Abstraction of a temporary file tied to a scope managed by <see cref="IScopedTempFileManager"/>. 
    /// Explicit call to `Dispose()` will delete the file immediately, otherwise the file will be deleted 
    /// when the owning <see cref="IScopedTempFileManager"/>'s `Dispose()` is called. This file may then 
    /// be written to, read, appended, etc... as needed within the lifetime of the manager.
    /// </summary>
    public interface IScopedTempFile : IDisposable
    {
        /// <summary>
        /// Opens the file for reading.
        /// </summary>
        /// <returns>A `Stream` suitable for reading from the start of the file.</returns>
        Stream OpenRead();

        /// <summary>
        /// Truncates the file (if already existing) and opens a fresh stream for writing.
        /// </summary>
        /// <returns>A `Stream` suitable for writing from the beginning of the file.</returns>
        Stream OpenWrite();

        /// <summary>
        /// Opens the file for appending to the end.
        /// </summary>
        /// <returns>A `Stream` suitable for appending to the file.</returns>
        Stream OpenAppend();
    }

    /// <summary>
    /// Base class for <see cref="IScopedTempFile"/> implementations.
    /// </summary>
    public abstract class ScopedTempFile : SafeDisposableWithFinalizer, IScopedTempFile
    {
        private readonly List<Action> _disposeActions = new List<Action>();

        /// <summary>
        /// Provide some behavior to be executed when this <see cref="ScopedTempFile"/> is being `Disposed`.
        /// </summary>
        /// <param name="action">The additional `Action` to execute during `Dispose()`</param>
        public void AddDisposeAction(Action action)
        {
            _disposeActions.Add(action);
        }

        protected override void DisposeManagedObjects()
        {
            foreach(var action in _disposeActions)
            {
                action?.Invoke();
            }
        }

        protected override void DisposeUnmanagedObjects()
        {
            // Don't force children to implement this if they don't have unmanaged resources to dispose
        }

        protected override void NullifyLargeFields()
        {
            base.NullifyLargeFields();
            _disposeActions.Clear();
        }

        public abstract Stream OpenAppend();
        public abstract Stream OpenRead();
        public abstract Stream OpenWrite();
    }
}
