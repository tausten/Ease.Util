//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ease.Util.Tests.Extensions
{
    class EnumExtensionsTests
    {
        [Test]
        public void GetAllEnumValuesFor_Returns_Typed_Values()
        {
            // Arrange
            // Act
            var result = EnumExtensions.GetAllEnumValuesFor<SomeEnumType>();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<SomeEnumType>>();
        }

        [Test]
        public void GetAllEnumValuesFor_Returns_All_Values_From_The_Enum()
        {
            // Arrange
            var expected = Enum.GetValues(typeof(SomeEnumType)).Cast<SomeEnumType>().ToList();

            // Act
            var result = EnumExtensions.GetAllEnumValuesFor<SomeEnumType>();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void GetAllEnumValues_Returns_Typed_Values()
        {
            // Arrange
            SomeFlagEnumType reference = default;

            // Act
            var result = reference.GetAllEnumValues();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<SomeFlagEnumType>>();
        }

        [Test]
        public void GetAllEnumValues_Returns_All_Values_From_The_Enum()
        {
            // Arrange
            SomeFlagEnumType reference = default;
            var expected = Enum.GetValues(typeof(SomeFlagEnumType)).Cast<SomeFlagEnumType>().ToList();

            // Act
            var result = reference.GetAllEnumValues();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
