//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using NUnit.Framework;
using FluentAssertions;

namespace Ease.Util.Tests.Disposably
{
    public class ScopedTests
    {
        [Test]
        public void EntryAction_Is_Called()
        {
            // Arrange
            var wasCalled = false;

            // Act
            using (var scope = new Scoped(entryAction: () => wasCalled = true))
            {
                // Assert
                wasCalled.Should().BeTrue();
            }
        }

        [Test]
        public void ExitAction_Is_Called_On_Dispose()
        {
            // Arrange
            var wasCalled = false;

            // Act
            using (var scope = new Scoped(exitAction: () => wasCalled = true))
            {
                wasCalled.Should().BeFalse("exitAction should not be called until Dispose() is called.");
                scope.Dispose();

                // Assert
                wasCalled.Should().BeTrue();
            }
        }
    }
}