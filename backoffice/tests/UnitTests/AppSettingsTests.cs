using Microsoft.Extensions.Configuration;

namespace tests
{
    public class AppSettingsUnitTests
    {

    [Fact]
    public void ReadAppSettingsFile_ShouldReadFileContents()
    {
        // Given
        string filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        // When
        string fileContents = File.ReadAllText(filePath);

        // Then
        Assert.NotEmpty(fileContents);
    }

    }
}
