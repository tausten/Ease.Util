//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System.IO;
using Ease.Util.Temp.Memory;
using FluentAssertions;
using NUnit.Framework;

namespace Ease.Util.Tests.Temp
{
    public class LocalScopedTempFileTests : ScopedTempFileTests<LocalScopedTempFileManager> { }

    public class AlternateDirectoryLocalScopedTempFileTests : ScopedTempFileTests<LocalScopedTempFileManager>
    {
        private DirectoryInfo _tempDir;

        protected override LocalScopedTempFileManager NewManager()
        {
            return new LocalScopedTempFileManager(_tempDir);
        }

        [SetUp]
        public override void SetUp()
        {
            _tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            if (_tempDir.Exists)
            {
                _tempDir.Delete(recursive: true);
            }
        }

        [Test]
        public void Creates_Files_Under_Explicit_Directory()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                // Assert
                _tempDir.GetFiles("*").Length.Should().Be(1);
            }
        }

        [Test]
        public void Removes_Files_On_Explicit_Dispose_Of_File()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                // Assert
            }
            _tempDir.GetFiles("*").Length.Should().Be(0);
        }

        [Test]
        public void Removes_Folder_On_Dispose_Of_Manager()
        {
            // Arrange
            // Act
            using (Sut)
            using (var file = Sut.New())
            {
                // Assert
            }
            _tempDir.Exists.Should().BeFalse();
        }
    }
}
