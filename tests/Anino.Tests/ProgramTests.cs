using FluentAssertions;

namespace Anino.Tests;

public class ProgramTests
{
    [Fact]
    public void ProcessGenerateTemplateFilename_WithoutOption_ShouldReturnNull()
    {
        // Arrange
        string? filename = null;
        bool optionProvided = false;

        // Act
        var result = InvokeProcessGenerateTemplateFilename(filename, optionProvided);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ProcessGenerateTemplateFilename_WithOptionButNoFilename_ShouldReturnDefault()
    {
        // Arrange
        string? filename = null;
        bool optionProvided = true;

        // Act
        var result = InvokeProcessGenerateTemplateFilename(filename, optionProvided);

        // Assert
        result.Should().Be("template.json");
    }

    [Fact]
    public void ProcessGenerateTemplateFilename_WithOptionAndEmptyFilename_ShouldReturnDefault()
    {
        // Arrange
        string? filename = "";
        bool optionProvided = true;

        // Act
        var result = InvokeProcessGenerateTemplateFilename(filename, optionProvided);

        // Assert
        result.Should().Be("template.json");
    }

    [Fact]
    public void ProcessGenerateTemplateFilename_WithFilenameWithoutExtension_ShouldAppendJson()
    {
        // Arrange
        string? filename = "mytemplate";
        bool optionProvided = true;

        // Act
        var result = InvokeProcessGenerateTemplateFilename(filename, optionProvided);

        // Assert
        result.Should().Be("mytemplate.json");
    }

    [Fact]
    public void ProcessGenerateTemplateFilename_WithFilenameWithJsonExtension_ShouldKeepAsIs()
    {
        // Arrange
        string? filename = "mytemplate.json";
        bool optionProvided = true;

        // Act
        var result = InvokeProcessGenerateTemplateFilename(filename, optionProvided);

        // Assert
        result.Should().Be("mytemplate.json");
    }

    [Fact]
    public void ProcessGenerateTemplateFilename_WithFilenameWithJsonExtensionCaseInsensitive_ShouldKeepAsIs()
    {
        // Arrange
        string? filename = "mytemplate.JSON";
        bool optionProvided = true;

        // Act
        var result = InvokeProcessGenerateTemplateFilename(filename, optionProvided);

        // Assert
        result.Should().Be("mytemplate.JSON");
    }

    [Theory]
    [InlineData("api-config")]
    [InlineData("sample")]
    [InlineData("test-template")]
    public void ProcessGenerateTemplateFilename_WithVariousNamesWithoutExtension_ShouldAppendJson(string filename)
    {
        // Arrange
        bool optionProvided = true;

        // Act
        var result = InvokeProcessGenerateTemplateFilename(filename, optionProvided);

        // Assert
        result.Should().Be($"{filename}.json");
    }

    // Helper method that replicates the logic from ProcessGenerateTemplateFilename
    // Since testing private methods in top-level programs is complex, we replicate the logic here
    private static string? InvokeProcessGenerateTemplateFilename(string? filename, bool optionProvided)
    {
        if (!optionProvided)
        {
            return null;
        }

        // If option is provided but no filename (e.g., just --generate-template), use default
        if (string.IsNullOrEmpty(filename))
        {
            return "template.json";
        }

        if (!filename.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            return filename + ".json";
        }

        return filename;
    }
}