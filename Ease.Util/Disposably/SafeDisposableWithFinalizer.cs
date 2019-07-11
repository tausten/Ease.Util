//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;

namespace Ease.Util.Disposably
{
    public abstract class SafeDisposableWithFinalizer : SafeDisposable
    {
        // Intentionally providing default no-op implementation here as we may only have unmanaged resources to dispose of.
        protected override void DisposeManagedObjects() { }

        /// <summary>
        /// Provide implementation to dispose of unmanaged objects. If you don't have unmanaged objects to clean up, then 
        /// inherit directly from SafeDisposable class and override the DisposeManagedObjects() method instead.
        /// 
        /// NO thread synchronization required as this method is guaranteed to not be re-entered.
        /// 
        /// CAUTION: Your implementation must be kept safe to call more than once, as this may occur if Dispose is called
        /// explicitly and the Finalizer is eventually called. This double-call is not guaranteed but may occur.
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
