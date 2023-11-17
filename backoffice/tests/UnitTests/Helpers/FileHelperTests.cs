using Droits.Helpers;

namespace Droits.Tests.UnitTests.Helpers
{
    public class FileHelperTests
    {
        [Theory]
        [InlineData("example.txt", "text/plain")]
        [InlineData("document.pdf", "application/pdf")]
        [InlineData("image.jpg", "image/jpeg")]
        [InlineData("audio.mp3", "audio/mpeg")]
        [InlineData("unknown.extension", "application/octet-stream")]
        public void GetContentType_ShouldReturnCorrectContentType(string fileName, string expectedContentType)
        {
            // Act
            var result = FileHelper.GetContentType(fileName);

            // Assert
            Assert.Equal(expectedContentType, result, StringComparer.OrdinalIgnoreCase);
        }

        [Theory]
        [MemberData(nameof(InvalidFileNames))]
        public void GetContentType_ShouldReturnDefaultContentTypeForInvalidFileNames(string fileName)
        {
            // Act
            var result = FileHelper.GetContentType(fileName);

            // Assert
            Assert.Equal("application/octet-stream", result, StringComparer.OrdinalIgnoreCase);
        }

        public static IEnumerable<object?[]> InvalidFileNames =>
            new List<object?[]>
            {
                new object?[] { null },
                new object[] { string.Empty },
                new object?[] { "   " },
            };
    }
}