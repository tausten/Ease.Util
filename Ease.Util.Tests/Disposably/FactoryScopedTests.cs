//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Disposably;
using NUnit.Framework;
using FluentAssertions;

namespace Ease.Util.Tests.Disposably
{
    public class FactoryScopedTests
    {
        [Test]
        public void AllocateFunction_Is_Called_After_Access_To_Instance()
        {
            // Arrange
            var wasCalled = false;

            // Act
            using (var scope = new FactoryScoped<string>(
                allocateFunction: () => { wasCalled = true; return "Hello World!"; },
                releaseAction: s => { }))
            {
                wasCalled.Should().BeFalse();
                var thing = scope.Instance;

                // Assert
                wasCalled.Should().BeTrue();
            }
        }

        [Test]
        public void AllocateFunction_Is_Not_Called_If_No_Access_To_Instance()
        {
            // Arrange
            var wasCalled = false;

            // Act
            using (var scope = new FactoryScoped<string>(
                allocateFunction: () => { wasCalled = true; return "Hello World!"; },
                releaseAction: s => { }))
            {
                wasCalled.Should().BeFalse();
            }

            // Assert
            wasCalled.Should().BeFalse();
        }

        [Test]
        public void ReleaseAction_Is_Called_On_Dispose()
        {
            // Arrange
            var wasCalled = false;

            // Act
            using (var scope = new FactoryScoped<string>(
                allocateFunction: () => { return "Hello World!"; },
                releaseAction: s => { wasCalled = true; }))
            {
                wasCalled.Should().BeFalse("releaseAction should not be called until Dispose() is called.");
                var thing = scope.Instance;
                scope.Dispose();

                // Assert
                wasCalled.Should().BeTrue();
            }
        }

        [Test]
        public void ReleaseAction_Is_Passed_The_Instance_Returned_From_FactoryFunction()
        {
            // Arrange
            var theObjectToReturnFromFactory = new object();
            object theObjectReturnedFromInstanceMember = null;
            object theObjectPassedToReleaseAction = null;

            // Act
            using (var scope = new FactoryScoped<object>(
                allocateFunction: () => { return theObjectToReturnFromFactory; },
                releaseAction: s => { theObjectPassedToReleaseAction = s; }))
            {
                theObjectReturnedFromInstanceMember = scope.Instance;
            }

            // Assert
            theObjectReturnedFromInstanceMember.Should().NotBeNull();
            theObjectReturnedFromInstanceMember.Should().BeSameAs(theObjectToReturnFromFactory);
            theObjectPassedToReleaseAction.Should().BeSameAs(theObjectReturnedFromInstanceMember);
        }

        [Test]
        public void Instance_Returns_Same_Value_On_Multiple_Accesses()
        {
            // Arrange
            // Act
            using (var scope = new FactoryScoped<object>(
                allocateFunction: () => { return new object(); },
                releaseAction: s => { }))
            {
                var firstAccess = scope.Instance;
                var secondAccess = scope.Instance;

                // Assert
                secondAccess.Should().BeSameAs(firstAccess);
            }
        }
    }
}