using Droits.Helpers;
using Droits.Models.Entities;
using Droits.Repositories;

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
        
        
        [Fact]
        public void GetFileKey_WreckMaterial_ReturnsCorrectKey()
        {
            // Arrange
            var droitFile = new DroitFile
            {
                WreckMaterial = new WreckMaterial { DroitId = Guid.NewGuid(), Id = Guid.NewGuid() },
                Id = Guid.NewGuid()
            };
            const string filename = "test1.test";
            
            // Act
            var result = FileHelper.GetFileKey(droitFile, filename);

            // Assert
            var expectedKey =
                $"Droits/{droitFile.WreckMaterial.DroitId}/WreckMaterials/{droitFile.WreckMaterialId}/DroitFiles/{droitFile.Id}_{filename}";
            Assert.Equal(expectedKey, result);
        }
        
        [Fact]
        public void GetFileKey_DroitNote_ReturnsCorrectKey()
        {
            // Arrange
            var droitFile = new DroitFile
            {
                Note = new Note { DroitId = Guid.NewGuid(), Id = Guid.NewGuid() },
                Id = Guid.NewGuid()
            };
            const string filename = "test2.test";
            
            // Act
            var result = FileHelper.GetFileKey(droitFile, filename);

            // Assert
            var expectedKey =
                $"Droits/{droitFile.Note.DroitId}/Notes/{droitFile.Note.Id}/DroitFiles/{droitFile.Id}_{filename}";
            Assert.Equal(expectedKey, result);
        }
        
        [Fact]
        public void GetFileKey_SalvorNote_ReturnsCorrectKey()
        {
            // Arrange
            var droitFile = new DroitFile
            {
                Note = new Note { SalvorId = Guid.NewGuid(), Id = Guid.NewGuid() },
                Id = Guid.NewGuid()
            };
            const string filename = "test3.test";
            
            // Act
            var result = FileHelper.GetFileKey(droitFile, filename);

            // Assert
            var expectedKey =
                $"Salvors/{droitFile.Note.SalvorId}/Notes/{droitFile.Note.Id}/DroitFiles/{droitFile.Id}_{filename}";
            Assert.Equal(expectedKey, result);
        }
        
        [Fact]
        public void GetFileKey_WreckNote_ReturnsCorrectKey()
        {
            // Arrange
            var droitFile = new DroitFile
            {
                Note = new Note { WreckId = Guid.NewGuid(), Id = Guid.NewGuid() },
                Id = Guid.NewGuid()
            };
            const string filename = "test4.test";
            
            // Act
            var result = FileHelper.GetFileKey(droitFile, filename);

            // Assert
            var expectedKey =
                $"Wrecks/{droitFile.Note.WreckId}/Notes/{droitFile.Note.Id}/DroitFiles/{droitFile.Id}_{filename}";
            Assert.Equal(expectedKey, result);
        }
        
        [Fact]
        public void GetFileKey_LetterNote_ReturnsCorrectKey()
        {
            // Arrange
            var droitFile = new DroitFile
            {
                Note = new Note { LetterId = Guid.NewGuid(), Id = Guid.NewGuid() },
                Id = Guid.NewGuid()
            };
            const string filename = "test5.test";
            
            // Act
            var result = FileHelper.GetFileKey(droitFile, filename);

            // Assert
            var expectedKey =
                $"Letters/{droitFile.Note.LetterId}/Notes/{droitFile.Note.Id}/DroitFiles/{droitFile.Id}_{filename}";
            Assert.Equal(expectedKey, result);
        }
        
        
        [Fact]
        public void GetFileKey_UnattachedNote_ThrowsInvalidOperationException()
        {
            // Arrange
            var droitFile = new DroitFile
            {
                Note = new Note { Id = Guid.NewGuid() },
                Id = Guid.NewGuid()
            };
            const string filename = "test6.test";
            
            // Assert

            Assert.Throws<InvalidOperationException>(() => FileHelper.GetFileKey(droitFile, filename));
        }

        [Fact]
        public void GetFileKey_NoFilename_ReturnsCorrectKey()
        {
            // Arrange
            var droitFile = new DroitFile
            {
                Note = new Note { LetterId = Guid.NewGuid(), Id = Guid.NewGuid() },
                Id = Guid.NewGuid()
            };
            
            // Act
            var result = FileHelper.GetFileKey(droitFile, null);

            // Assert
            var expectedKey =
                $"Letters/{droitFile.Note.LetterId}/Notes/{droitFile.Note.Id}/DroitFiles/{droitFile.Id}_";
            Assert.Equal(expectedKey, result);
        }
        
        [Fact]
        public void GetFileKey_NoNoteOrWM_ReturnsNull()
        {
            // Arrange
            var droitFile = new DroitFile
            {
                Id = Guid.NewGuid()
            };
            
            // Act
            var result = FileHelper.GetFileKey(droitFile, null);

            // Assert
            Assert.Null(result);
        }
    }
}