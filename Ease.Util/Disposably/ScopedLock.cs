//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Ease.Util.Disposably
{
    /// <summary>
    /// A class for mapping locking into a Disposable pattern. Normally, you should just use `lock` for your traditional 
    /// thread synchronization, but this class helps in the case where you wish to allow a user of your class to "borrow" an 
    /// internal guard object so they may lock it for the duration of a `using` block. Rather than just handing out your
    /// inner guard object and losing control of it, you can narrow the exposure to the using block, and continue to hide
    /// the guard object itself.
    /// 
    /// See <see cref="SafeDisposable"/> for an example of one special case where this can be used to close a race condition.
    /// 
    /// <code>
    /// class SomeClass
    /// {
    ///     private readonly object _guard = new object();
    ///     // ... a bunch of functionality that relies upon the _guard ...
    ///     
    ///     /// 
    ///     /// The property to allow clients of your class to borrow your 
    ///     /// _guard object briefly.
    ///     /// 
    ///     public ScopedLock TemporaryLock { get { return new ScopedLock(_guard); } }
    /// }
    /// 
    /// SomeClass theThing; // obtained however is appropriate for your situation
    /// 
    /// // ... client now wishes to make sure their block of code can execute to 
    /// // completion before any of your methods can acquire the lock...
    /// 
    /// using (theThing.TemporaryLock)
    /// {
    ///     // ... go ahead and do stuff knowing that theThing's lock won't be acquired 
    ///     // before this scope is exited.
    /// }
    /// </code>
    /// </summary>
    public class ScopedLock : Scoped
    {
        /// <summary>
        /// Construct a disposable objection which will acquire a lock on the passed <paramref name="guardObject"/> on 
        /// construction, and then release the lock on it when `Dispose()` is called.
        /// </summary>
        /// <param name="guardObject">The object to lock and release</param>
        public ScopedLock(object guardObject) : base(() => Monitor.Enter(guardObject), () => Monitor.Exit(guardObject))
        { }
    }
}
