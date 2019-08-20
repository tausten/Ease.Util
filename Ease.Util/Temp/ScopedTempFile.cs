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
    /// </summary>
    public interface IScopedTempFile : IDisposable
    {
        /// <summary>
        /// Opens the file for reading.
        /// </summary>
        /// <returns></returns>
        Stream OpenRead();

        /// <summary>
        /// Truncates the file (if already existing) and opens a fresh stream for writing.
        /// </summary>
        /// <returns></returns>
        Stream OpenWrite();

        /// <summary>
        /// Opens the file for appending to the end.
        /// </summary>
        /// <returns></returns>
        Stream OpenAppend();
    }

    /// <summary>
    /// Base implementation for <see cref="IScopedTempFile"/>.
    /// </summary>
    public abstract class ScopedTempFile : SafeDisposableWithFinalizer, IScopedTempFile
    {
        private readonly List<Action> _disposeActions = new List<Action>();

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
