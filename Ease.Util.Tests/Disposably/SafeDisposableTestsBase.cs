//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace Ease.Util.Tests.Disposably
{
    public interface ITestSafeDisposableCounts
    {
        int DisposeManagedObjectsCount { get; }
        int NullifyLargeFieldsCount { get; }
    }

    public abstract class SafeDisposableTestsBase<TDisposable> where TDisposable : SafeDisposable, ITestSafeDisposableCounts, IDisposable, new()
    {
        [Test]
        public void IsDisposed_Is_False_Initially()
        {
            // Arrange
            using (var cut = new TDisposable())
            {
                // Act
                // Assert
                cut.IsDisposed.Should().BeFalse();
            }
        }

        [Test]
        public void CheckDisposed_Does_Not_Throw_If_Not_Disposed()
        {
            // Arrange
            using (var cut = new TDisposable())
            {
                // Act
                // Assert
                cut.CheckDisposed();
            }
        }

        [Test]
        public void IsDisposed_Is_True_After_Disposed()
        {
            // Arrange
            using (var cut = new TDisposable())
            {
                // Act
                cut.Dispose();

                // Assert
                cut.IsDisposed.Should().BeTrue();
            }
        }

        [Test]
        public void CheckDisposed_Throws_ObjectDisposedException_After_Disposed()
        {
            // Arrange
            using (var cut = new TDisposable())
            {
                // Act
                cut.Dispose();

                // Assert
                cut.Invoking(x => x.CheckDisposed())
                    .Should().Throw<ObjectDisposedException>();
            }
        }

        [Test]
        public void DisposeManagedObjects_Is_Only_Called_Once_For_Repeated_Calls_To_Dispose()
        {
            // Arrange
            using (var cut = new TDisposable())
            {
                // Act
                cut.Dispose();
                cut.Dispose();
                cut.Dispose();

                // Assert
                cut.DisposeManagedObjectsCount.Should().Be(1);
            }
        }

        [Test]
        public void NullifyLargeFields_Is_Only_Called_Once_For_Repeated_Calls_To_Dispose()
        {
            // Arrange
            using (var cut = new TDisposable())
            {
                // Act
                cut.Dispose();
                cut.Dispose();
                cut.Dispose();

                // Assert
                cut.NullifyLargeFieldsCount.Should().Be(1);
            }
        }
    }
}