//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using Ease.Util.Extensions;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using System.IO;

namespace Ease.Util.Tests.Extensions
{
    public class AssemblyExtensionsTests
    {
        [Test]
        public void GetResourceBySuffixAsStream_Returns_Null_For_NonExistent()
        {
            // Arrange
            // Act
            using (var stream = Assembly.GetExecutingAssembly().GetResourceBySuffixAsStream("NON-EXISTENT-RESOURCE"))
            {
                // Assert
                stream.Should().BeNull();
            }
        }

        [TestCase("NonAmbiguous.txt")]
        [TestCase("NONAmbiGUOUS.txt")]
        public void GetResourceBySuffixAsStream_Returns_Stream_For_Existent(string resourceNameSuffix)
        {
            // Arrange
            // Act
            using (var stream = Assembly.GetExecutingAssembly().GetResourceBySuffixAsStream(resourceNameSuffix))
            {
                // Assert
                stream.Should().NotBeNull();
            }
        }

        private static string ReadAll(Stream stream)
        {
            using (stream)
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        [TestCase("ExplicitAmbiguous.txt", "Shortest")]
        [TestCase("ExplicitAMBIGUOUS.TXT", "Shortest")]
        [TestCase("EXPLICITAmbiguous.txt", "Case")]
        public void GetResourceBySuffixAsStream_Returns_Shortest_Path_Matching_Resource(string resourceNameSuffix, string expectedContents)
        {
            // Arrange
            // Act
            var result = ReadAll(Assembly.GetExecutingAssembly().GetResourceBySuffixAsStream(resourceNameSuffix));

            // Assert
            result.Should().Be(expectedContents);
        }

        [Test]
        public void GetResourceBySuffixAsString_Returns_Null_For_NonExistent()
        {
            // Arrange
            // Act
            var result = Assembly.GetExecutingAssembly().GetResourceBySuffixAsString("NON-EXISTENT-RESOURCE");

            // Assert
            result.Should().BeNull();
        }

        [TestCase("NonAmbiguous.txt")]
        [TestCase("NONAmbiGUOUS.txt")]
        public void GetResourceBySuffixAsString_Returns_Stream_For_Existent(string resourceNameSuffix)
        {
            // Arrange
            // Act
            var result = Assembly.GetExecutingAssembly().GetResourceBySuffixAsString(resourceNameSuffix);

            // Assert
            result.Should().NotBeNull();
        }

        [TestCase("ExplicitAmbiguous.txt", "Shortest")]
        [TestCase("ExplicitAMBIGUOUS.TXT", "Shortest")]
        [TestCase("EXPLICITAmbiguous.txt", "Case")]
        public void GetResourceBySuffixAsString_Returns_Shortest_Path_Matching_Resource(string resourceNameSuffix, string expectedContents)
        {
            // Arrange
            // Act
            var result = Assembly.GetExecutingAssembly().GetResourceBySuffixAsString(resourceNameSuffix);

            // Assert
            result.Should().Be(expectedContents);
        }
    }
}
