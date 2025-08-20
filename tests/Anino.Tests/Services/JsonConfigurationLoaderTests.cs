using System.Text.Json;
using Anino.Services;
using FluentAssertions;

namespace Anino.Tests.Services;

public class JsonConfigurationLoaderTests
{
    private readonly JsonConfigurationLoader _loader;
    private readonly string _testFilesDirectory;

    public JsonConfigurationLoaderTests()
    {
        _loader = new JsonConfigurationLoader();
        _testFilesDirectory = Path.Combine(Path.GetTempPath(), "AninoTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testFilesDirectory);
    }

    [Fact]
    public void LoadEndpoints_WithValidJson_ShouldReturnEndpoints()
    {
        // Arrange
        var jsonContent = """
        [
            {
                "path": "/api/users",
                "method": "GET",
                "statusCode": 200,
                "response": [{"id": 1, "name": "John"}]
            }
        ]
        """;

        var testFile = Path.Combine(_testFilesDirectory, "valid.json");
        File.WriteAllText(testFile, jsonContent);
        var fileInfo = new FileInfo(testFile);

        // Act
        var result = _loader.LoadEndpoints(fileInfo);

        // Assert
        result.Should().HaveCount(1);
        result[0].Path.Should().Be("/api/users");
        result[0].Method.Should().Be("GET");
        result[0].StatusCode.Should().Be(200);
        result[0].Response.ValueKind.Should().Be(JsonValueKind.Array);
    }

    [Fact]
    public void LoadEndpoints_WithNonExistentFile_ShouldThrowFileNotFoundException()
    {
        // Arrange
        var nonExistentFile = new FileInfo(Path.Combine(_testFilesDirectory, "nonexistent.json"));

        // Act & Assert
        var act = () => _loader.LoadEndpoints(nonExistentFile);
        act.Should().Throw<FileNotFoundException>()
            .WithMessage("Configuration file '*nonexistent.json' not found.");
    }

    [Fact]
    public void LoadEndpoints_WithInvalidJson_ShouldThrowJsonException()
    {
        // Arrange
        var invalidJson = "{ invalid json }";
        var testFile = Path.Combine(_testFilesDirectory, "invalid.json");
        File.WriteAllText(testFile, invalidJson);
        var fileInfo = new FileInfo(testFile);

        // Act & Assert
        var act = () => _loader.LoadEndpoints(fileInfo);
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void LoadEndpoints_WithEmptyArray_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var emptyJson = "[]";
        var testFile = Path.Combine(_testFilesDirectory, "empty.json");
        File.WriteAllText(testFile, emptyJson);
        var fileInfo = new FileInfo(testFile);

        // Act & Assert
        var act = () => _loader.LoadEndpoints(fileInfo);
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("No endpoints found in the configuration file or the file is invalid.");
    }

    [Fact]
    public void LoadEndpoints_WithCaseInsensitiveProperties_ShouldWork()
    {
        // Arrange
        var jsonContent = """
        [
            {
                "PATH": "/api/test",
                "METHOD": "POST",
                "STATUSCODE": 201,
                "RESPONSE": {"message": "created"}
            }
        ]
        """;

        var testFile = Path.Combine(_testFilesDirectory, "case-insensitive.json");
        File.WriteAllText(testFile, jsonContent);
        var fileInfo = new FileInfo(testFile);

        // Act
        var result = _loader.LoadEndpoints(fileInfo);

        // Assert
        result.Should().HaveCount(1);
        result[0].Path.Should().Be("/api/test");
        result[0].Method.Should().Be("POST");
        result[0].StatusCode.Should().Be(201);
    }
}