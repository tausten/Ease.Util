//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Extensions;
using FluentAssertions;
using NUnit.Framework;
using System.IO;
using System;

namespace Ease.Util.Tests.Extensions
{
    public class DirectoryInfoExtensionsTests
    {
        private DirectoryInfo _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        }

        [TearDown]
        public void TearDown()
        {
            if (_sut.Exists)
            {
                _sut.Delete(recursive: true);
            }
        }

        [Test]
        public void GetTempFileName_Throws_ArgumentNullException_For_Null_DirectoryInfo()
        {
            // Arrange
            DirectoryInfo sut = null;

            // Act
            sut.Invoking(x => x.GetTempFileName())
            // Assert
                .Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("directoryInfo");
        }

        [Test]
        public void GetTempFileName_Creates_The_ZeroByte_File()
        {
            // Arrange
            // Act
            var result = _sut.GetTempFileName();

            // Assert
            var info = new FileInfo(result);
            info.Exists.Should().BeTrue();
            info.Length.Should().Be(0);
        }

        [Test]
        public void GetTempFileName_Creates_The_File_Under_The_Directory()
        {
            // Arrange
            // Act
            var result = _sut.GetTempFileName();

            // Assert
            var files = _sut.GetFiles("*");
            files.Length.Should().Be(1);
            files[0].FullName.Should().Be(result);
        }

        [Test]
        public void GetTempFileName_Creates_Different_Files_On_Multiple_Calls()
        {
            // Arrange
            // Act
            var result1 = _sut.GetTempFileName();
            var result2 = _sut.GetTempFileName();

            // Assert
            result1.Should().NotBe(result2);
        }
    }
}
