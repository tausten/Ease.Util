//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;

namespace Ease.Util.Disposably
{
    /// <summary>
    /// Base class for implementation of standard IDisposable pattern in threadsafe, multi-Dispose() protected manner. This 
    /// variant adds support for finalization which is important for classes that use unmanaged resources and need to ensure
    /// they are cleaned up in the last-ditch finalization flow if they haven't been before.
    /// </summary>
    public abstract class SafeDisposableWithFinalizer : SafeDisposable
    {
        // Intentionally providing default no-op implementation here as we may only have unmanaged resources to dispose of.
        protected override void DisposeManagedObjects() { }

        /// <summary>
        /// Provide implementation to dispose of unmanaged objects. If you don't have unmanaged objects to clean up, then 
        /// inherit directly from SafeDisposable class and override the DisposeManagedObjects() method instead.
        /// 
        /// NO thread synchronization required as this method is guaranteed to not be re-entered.
        /// </summary>
        protected abstract void DisposeUnmanagedObjects();

        /// <summary>
        /// The only case where this would need to be overridden - i.e. to implement the Finalizer support while hiding 
        /// all the gory details from the users of the classes.
        /// </summary>
        internal override void FinalCleanUp()
        {
            DisposeUnmanagedObjects();
            base.FinalCleanUp();
            GC.SuppressFinalize(this);
        }

        ~SafeDisposableWithFinalizer()
        {
           Dispose(false);
        }
    }
}
