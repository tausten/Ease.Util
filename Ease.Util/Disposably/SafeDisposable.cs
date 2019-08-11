//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;

namespace Ease.Util.Disposably
{
    /// <summary>
    /// Base class for implementation of standard `IDisposable` pattern in threadsafe, multi-Dispose() protected manner.
    /// </summary>
    public abstract class SafeDisposable : IDisposable
    {
        /// <summary>
        /// true if the object has been disposed, false otherwise.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                var isDisposed = _isDisposed;
                if (!_isDisposed)
                {
                    // This way, we only ever use the lock in case where race condition could happen.
                    lock (_isDisposedGuard)
                    {
                        isDisposed = _isDisposed;
                    }
                }
                return isDisposed;
            }
        }

        /// <summary>
        /// Throws an `ObjectDisposedException` if the object has already been disposed. This can be used to avoid 
        /// starting something if the object has already been disposed.
        /// NOTE: This is not a guarantee that a separate thread doesn't dispose the object after the check has been 
        /// performed. If you wish to guarantee the object will not be disposed while you're operating on it, then 
        /// use the <see cref="Lock"/> property in a `using` statement.
        /// </summary>
        public void CheckDisposed()
        {
            if(IsDisposed)
            {
                throw new ObjectDisposedException(ToString());
            }
        }

        /// <summary>
        /// A fresh `IDisposable` that will acquire a lock preventing this object from being Disposed until
        /// the returned <see cref="ScopedLock"/> has itself been Disposed. This is intended to _always_ be used 
        /// with `using`, to allow a block of code to run to completion before another thread can Dispose the 
        /// parent object.
        /// 
        /// <code>
        /// var theParentDisposable;  // some child of <see cref="SafeDisposable"/>
        /// using(theParentDisposable.Lock)
        /// {
        ///     // ... ok... the code in this block will execute to completion without concern about a 
        ///     // separate thread calling Dispose
        /// }
        /// // ... theParentDisposable itself is _not_ Disposed here...  we've only released its internal 
        /// // guard...   we (and anyone else) are still free to use it until it is itself Disposed.
        /// </code>
        /// </summary>
        public ScopedLock Lock => new ScopedLock(_isDisposedGuard);

        #region IDisposable Support
        private readonly object _isDisposedGuard = new object();
        private bool _isDisposed = false; // To detect redundant calls

        protected void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                lock (_isDisposedGuard)
                {
                    // Double-check that we weren't trying concurrent disposal and just got in after the other finished.
                    if (!_isDisposed)
                    {
                        if (disposing)
                        {
                            DisposeManagedObjects();
                        }

                        FinalCleanUp();
                        _isDisposed = true;
                    }
                }
            }
        }

        /// <summary>
        /// Override and provide implementation to dispose of managed objects.
        /// NO thread synchronization required as this method is guaranteed to not be re-entered.
        /// </summary>
        protected abstract void DisposeManagedObjects();

        /// <summary>
        /// You should not need to override this.. if tempted, have a look at the <see cref="SafeDisposableWithFinalizer"/> class instead.
        /// Intentionally `internal` so that only the `SafeDisposableWithFinalizer` can see and override it.
        /// </summary>
        internal virtual void FinalCleanUp()
        {
            NullifyLargeFields();
        }

        /// <summary>
        /// Override and provide implementation to explicitly nullify fields referencing large objects.
        /// NO thread synchronization required as this method is guaranteed to not be re-entered.
        /// </summary>
        protected virtual void NullifyLargeFields() { }

        /// <summary>
        /// Safely disposes the object. Can safely be called multiple times and will only invoke the dispose work 
        /// once as a direct result. This implementation is also threadsafe.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
