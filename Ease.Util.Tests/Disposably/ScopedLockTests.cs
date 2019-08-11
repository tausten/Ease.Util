//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using NUnit.Framework;
using FluentAssertions;
using System.Threading;

namespace Ease.Util.Tests.Disposably
{
    public class ScopedLockTests
    {
        private object _guard;

        [SetUp]
        public void SetUp()
        {
            _guard = new object();
        }

        [Test]
        public void Lock_Is_Acquired()
        {
            // Arrange
            Monitor.IsEntered(_guard).Should().BeFalse();

            // Act
            using (new ScopedLock(_guard))
            {
                // Assert
                Monitor.IsEntered(_guard).Should().BeTrue();
            }
        }

        [Test]
        public void Lock_Is_Released()
        {
            // Arrange
            // Act
            using (new ScopedLock(_guard))
            {
                // Assert
                Monitor.IsEntered(_guard).Should().BeTrue();
            }
            Monitor.IsEntered(_guard).Should().BeFalse();
        }
    }
}