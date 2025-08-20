using Anino.Configuration;
using FluentAssertions;

namespace Anino.Tests.Configuration;

public class AninoOptionsTests
{
    [Fact]
    public void DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var options = new AninoOptions();

        // Assert
        options.File.Should().BeNull();
        options.Port.Should().Be(6000);
        options.Latency.Should().Be(0);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var testFile = new FileInfo("test.json");
        var options = new AninoOptions();

        // Act
        options.File = testFile;
        options.Port = 8080;
        options.Latency = 500;

        // Assert
        options.File.Should().Be(testFile);
        options.Port.Should().Be(8080);
        options.Latency.Should().Be(500);
    }
}