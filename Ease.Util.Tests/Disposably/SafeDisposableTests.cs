//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using static Ease.Util.Tests.Disposably.SafeDisposableTests;

namespace Ease.Util.Tests.Disposably
{
    public class SafeDisposableTests : SafeDisposableTestsBase<TestDisposable>
    {
        public class TestDisposable : SafeDisposable, ITestSafeDisposableCounts
        {
            public int DisposeManagedObjectsCount { get; private set; }
            protected override void DisposeManagedObjects()
            {
                ++DisposeManagedObjectsCount;
            }

            public int NullifyLargeFieldsCount { get; private set; }
            protected override void NullifyLargeFields()
            {
                try
                {
                    base.NullifyLargeFields();
                }
                finally
                {
                    ++NullifyLargeFieldsCount;
                }
            }
        }
    }
}