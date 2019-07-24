//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using FluentAssertions;
using NUnit.Framework;
using static Ease.Util.Tests.Disposably.SafeDisposableWithFinalizerTests;

namespace Ease.Util.Tests.Disposably
{
    public class SafeDisposableWithFinalizerTests : SafeDisposableTestsBase<TestDisposableWithFinalizer>
    {
        public class TestDisposableWithFinalizer : SafeDisposableWithFinalizer, ITestSafeDisposableCounts
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

            public int DisposeUnmanagedObjectsCount { get; private set; }
            protected override void DisposeUnmanagedObjects()
            {
                ++DisposeUnmanagedObjectsCount;
            }
        }

        [Test]
        public void DisposeUnmanagedObjects_Is_Only_Called_Once_For_Repeated_Calls_To_Dispose()
        {
            // Arrange
            using (var cut = new TestDisposableWithFinalizer())
            {
                // Act
                cut.Dispose();
                cut.Dispose();
                cut.Dispose();

                // Assert
                cut.DisposeUnmanagedObjectsCount.Should().Be(1);
            }
        }
    }
}