//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;

namespace Ease.Util.Disposably
{
    /// <summary>
    /// A utility class for managing lifetime of factory pattern managed resources that require to be released in some way other than
    /// by the simple Dispose pattern. The allocation is done lazily such that if the .Instance member is not accessed, then neither 
    /// `allocateFunction` nor `releaseAction` will be called.
    /// </summary>
    /// <typeparam name="T">The Type of object the factory will manage.</typeparam>
    public class FactoryScoped<T> : SafeDisposableWithFinalizer
        where T : class
    {
        private Lazy<T> _lazyInstance;
        private readonly Action<T> _releaseAction;

        /// <summary>
        /// Will use the `allocateFunction` lazily on initial access to the `.Instance` member to allocate a managed instance of 
        /// type `T` which will be subsequently be passed to `releaseAction` when the FactoryScoped object is Disposed.
        /// </summary>
        /// <param name="allocateFunction">The Func to execute in order to allocate the managed Instance.</param>
        /// <param name="releaseAction">The Action to execute on Dispose() or finalization.</param>
        public FactoryScoped(Func<T> allocateFunction, Action<T> releaseAction)
        {
            _lazyInstance = new Lazy<T>(allocateFunction ?? throw new ArgumentNullException(nameof(allocateFunction)));
            _releaseAction = releaseAction ?? throw new ArgumentNullException(nameof(releaseAction));

        }

        /// <summary>
        /// The instance of T managed by this FactoryScoped object.
        /// </summary>
        public T Instance => _lazyInstance.Value;

        protected override void DisposeUnmanagedObjects()
        {
            if(_lazyInstance.IsValueCreated)
            {
                _releaseAction(Instance);
            }
        }
    }
}
