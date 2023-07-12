namespace Droits.Tests.UnitTests;

public class AppSettingsUnitTests
{
    [Fact]
    public void ReadAppSettingsFile_ShouldReadFileContents()
    {
        // Given
        var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        // When
        var fileContents = File.ReadAllText(filePath);

        // Then
        Assert.NotEmpty(fileContents);
    }
}