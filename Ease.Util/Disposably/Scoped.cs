//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;

namespace Ease.Util.Disposably
{
    /// <summary>
    /// A utility class for managing ad-hoc scope states.
    /// </summary>
    public class Scoped : SafeDisposableWithFinalizer
    {
        private readonly Action _exitAction;

        /// <summary>
        /// Will execute the `entryAction` in constructor, and execute the `exitAction` on `Dispose()` or when object is finalized... 
        /// (and only once - not both times).
        /// </summary>
        /// <param name="entryAction">The Action to execute on construction.</param>
        /// <param name="exitAction">The Action to execute on `Dispose()` or finalization.</param>
        public Scoped(Action entryAction = null, Action exitAction = null)
        {
            entryAction?.Invoke();
            _exitAction = exitAction;

        }

        protected override void DisposeUnmanagedObjects()
        {
            _exitAction?.Invoke();
        }
    }
}
