//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System.Collections.Generic;
using System.IO;
using Ease.Util.Temp;
using FluentAssertions;
using NUnit.Framework;

namespace Ease.Util.Tests.Temp
{
    public class ScopedTempFileManagerTests
    {
        private class TestScopedTempFile : ScopedTempFile
        {
            public override Stream OpenAppend() { return new MemoryStream(); }

            public override Stream OpenRead() { return new MemoryStream(); }

            public override Stream OpenWrite() { return new MemoryStream(); }

            protected override void DisposeUnmanagedObjects() { }
        }

        private class TestScopedTempFileManager : ScopedTempFileManager
        {
            protected override ScopedTempFile AllocateFile()
            {
                return new TestScopedTempFile();
            }
        }

        private TestScopedTempFileManager _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new TestScopedTempFileManager();
        }

        [TearDown]
        public void TearDown()
        {
            _sut.Dispose();
        }

        [Test]
        public void Manager_Is_Disposable()
        {
            // Arrange
            using (_sut)
            {
                // Act
                // Assert
            }
        }

        [Test]
        public void New_Returns_Disposable_File()
        {
            // Arrange
            TestScopedTempFile theFile;
            using (var file = _sut.New())
            {
                // Act
                theFile = file as TestScopedTempFile;
                // Assert
                theFile.IsDisposed.Should().BeFalse();
            }
        }

        [Test]
        public void Dispose_Will_Dispose_Any_Returned_Files()
        {
            // Arrange
            var theFiles = new List<TestScopedTempFile>();
            using (_sut)
            {
                // Act
                theFiles.Add(_sut.New() as TestScopedTempFile);
                theFiles.Add(_sut.New() as TestScopedTempFile);
                theFiles.Add(_sut.New() as TestScopedTempFile);

                // Assert
                foreach (var file in theFiles)
                {
                    file.IsDisposed.Should().BeFalse();
                }
            }
            foreach (var file in theFiles)
            {
                file.IsDisposed.Should().BeTrue();
            }
        }
    }
}
