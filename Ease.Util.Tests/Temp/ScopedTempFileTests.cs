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
    public abstract class ScopedTempFileTests<TManager> where TManager : IScopedTempFileManager, new()
    {
        protected TManager Sut;

        /// <summary>
        /// Override if you need more complicated setup of your manager than just calling parameterless ctor.
        /// </summary>
        /// <returns></returns>
        protected virtual TManager NewManager()
        {
            return new TManager();
        }

        [SetUp]
        public virtual void SetUp()
        {
            Sut = NewManager();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Sut.Dispose();
        }

        [Test]
        public void Manager_Is_Disposable()
        {
            // Arrange
            using (Sut)
            {
                // Act
                // Assert
            }
        }

        [Test]
        public void New_Returns_Disposable_File()
        {
            // Arrange
            ScopedTempFile theFile;
            using (var file = Sut.New())
            {
                // Act
                theFile = file as ScopedTempFile;
                // Assert
                theFile.IsDisposed.Should().BeFalse();
            }
        }

        [Test]
        public void Dispose_Will_Dispose_Any_Returned_Files()
        {
            // Arrange
            var theFiles = new List<ScopedTempFile>();
            using (Sut)
            {
                // Act
                theFiles.Add(Sut.New() as ScopedTempFile);
                theFiles.Add(Sut.New() as ScopedTempFile);
                theFiles.Add(Sut.New() as ScopedTempFile);

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

        [Test]
        public void OpenAppend_Returns_NonNull_Writeable_Stream()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                using (var stream = file.OpenAppend())
                {
                    // Assert
                    stream.CanWrite.Should().BeTrue();
                }
            }
        }

        [Test]
        public void OpenRead_Returns_NonNull_Readable_Stream()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                using (var stream = file.OpenRead())
                {
                    // Assert
                    stream.CanRead.Should().BeTrue();
                }
            }
        }

        [Test]
        public void OpenWrite_Returns_NonNull_Writeable_Stream()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                using (var stream = file.OpenWrite())
                {
                    // Assert
                    stream.CanWrite.Should().BeTrue();
                }
            }
        }

        [Test]
        public void Can_Read_Empty_File()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                using (var reader = new StreamReader(file.OpenRead()))
                {
                    var result = reader.ReadToEnd();

                    // Assert
                    result.Should().BeEmpty();
                }
            }
        }

        [Test]
        public void Can_Write_Then_Read()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                const string theExpectedValue = "Hello world!";
                using (var writer = new StreamWriter(file.OpenWrite()))
                {
                    writer.Write(theExpectedValue);
                }

                using (var reader = new StreamReader(file.OpenRead()))
                {
                    var result = reader.ReadToEnd();

                    // Assert
                    result.Should().Be(theExpectedValue);
                }
            }
        }

        [Test]
        public void Can_Append_To_EmptyFile_Then_Read()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                const string theExpectedValue = "Hello world!";
                using (var writer = new StreamWriter(file.OpenAppend()))
                {
                    writer.Write(theExpectedValue);
                }

                using (var reader = new StreamReader(file.OpenRead()))
                {
                    var result = reader.ReadToEnd();

                    // Assert
                    result.Should().Be(theExpectedValue);
                }
            }
        }

        [Test]
        public void Can_Write_Then_Append_Then_Read()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                const string theWriteValue = "Hello world!";
                using (var writer = new StreamWriter(file.OpenWrite()))
                {
                    writer.Write(theWriteValue);
                }

                const string theAppendValue = " How's the day going?";
                using (var writer = new StreamWriter(file.OpenAppend()))
                {
                    writer.Write(theAppendValue);
                }

                using (var reader = new StreamReader(file.OpenRead()))
                {
                    var result = reader.ReadToEnd();

                    // Assert
                    result.Should().Be(theWriteValue + theAppendValue);
                }
            }
        }

        [Test]
        public void OpenWrite_Resets_Contents()
        {
            // Arrange
            // Act
            using (var file = Sut.New())
            {
                const string theWriteValue = "Hello world!";
                using (var writer = new StreamWriter(file.OpenWrite()))
                {
                    writer.Write(theWriteValue);
                }

                const string theSecondWriteValue = " How's the day going?";
                using (var writer = new StreamWriter(file.OpenWrite()))
                {
                    writer.Write(theSecondWriteValue);
                }

                using (var reader = new StreamReader(file.OpenRead()))
                {
                    var result = reader.ReadToEnd();

                    // Assert
                    result.Should().Be(theSecondWriteValue);
                }
            }
        }
    }
}
