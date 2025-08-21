using System.Text.Json;
using Anino.Services;
using FluentAssertions;

namespace Anino.Tests.Services;

public class DefinitionGeneratorTests : IDisposable
{
    private readonly DefinitionGenerator _definitionGenerator;
    private readonly string _testFilesDirectory;

    public DefinitionGeneratorTests()
    {
        _definitionGenerator = new DefinitionGenerator();
        _testFilesDirectory = Path.Combine(Path.GetTempPath(), "AninoDefinitionTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testFilesDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testFilesDirectory))
        {
            Directory.Delete(_testFilesDirectory, true);
        }
    }

    [Fact]
    public void GenerateDefinition_ShouldCreateValidJsonFile()
    {
        // Arrange
        var definitionFile = Path.Combine(_testFilesDirectory, "test-definition.json");

        // Act
        _definitionGenerator.GenerateDefinition(definitionFile);

        // Assert
        File.Exists(definitionFile).Should().BeTrue();
        
        var jsonContent = File.ReadAllText(definitionFile);
        jsonContent.Should().NotBeNullOrEmpty();
        
        // Verify it's valid JSON by deserializing
        var act = () => JsonSerializer.Deserialize<JsonElement[]>(jsonContent);
        act.Should().NotThrow();
    }

    [Fact]
    public void GenerateDefinition_ShouldContainCrudOperations()
    {
        // Arrange
        var definitionFile = Path.Combine(_testFilesDirectory, "crud-definition.json");

        // Act
        _definitionGenerator.GenerateDefinition(definitionFile);

        // Assert
        var jsonContent = File.ReadAllText(definitionFile);
        var endpoints = JsonSerializer.Deserialize<JsonElement[]>(jsonContent);

        endpoints.Should().NotBeNull();
        endpoints.Should().NotBeEmpty();

        // Check that we have GET, POST, PUT, DELETE operations
        var methods = endpoints
            .Select(e => e.GetProperty("method").GetString())
            .ToList();

        methods.Should().Contain("GET");
        methods.Should().Contain("POST");
        methods.Should().Contain("PUT");
        methods.Should().Contain("DELETE");
    }

    [Fact]
    public void GenerateDefinition_ShouldContainUsersEndpoints()
    {
        // Arrange
        var definitionFile = Path.Combine(_testFilesDirectory, "users-definition.json");

        // Act
        _definitionGenerator.GenerateDefinition(definitionFile);

        // Assert
        var jsonContent = File.ReadAllText(definitionFile);
        var endpoints = JsonSerializer.Deserialize<JsonElement[]>(jsonContent);

        // Check that we have user-related endpoints
        var paths = endpoints
            .Select(e => e.GetProperty("path").GetString())
            .ToList();

        paths.Should().Contain("/api/users");
        paths.Should().Contain("/api/users/{id}");
    }

    [Fact]
    public void GenerateDefinition_ShouldHaveProperStatusCodes()
    {
        // Arrange
        var definitionFile = Path.Combine(_testFilesDirectory, "status-definition.json");

        // Act
        _definitionGenerator.GenerateDefinition(definitionFile);

        // Assert
        var jsonContent = File.ReadAllText(definitionFile);
        var endpoints = JsonSerializer.Deserialize<JsonElement[]>(jsonContent);

        // Check status codes
        var statusCodes = endpoints
            .Select(e => e.GetProperty("statusCode").GetInt32())
            .ToList();

        statusCodes.Should().Contain(200); // GET requests
        statusCodes.Should().Contain(201); // POST requests
        statusCodes.Should().Contain(204); // DELETE requests
    }

    [Fact]
    public void GenerateDefinition_ShouldBeFormattedWithIndentation()
    {
        // Arrange
        var definitionFile = Path.Combine(_testFilesDirectory, "formatted-definition.json");

        // Act
        _definitionGenerator.GenerateDefinition(definitionFile);

        // Assert
        var jsonContent = File.ReadAllText(definitionFile);
        
        // Check that the JSON is properly formatted (indented)
        jsonContent.Should().Contain("  "); // Should contain indentation
        jsonContent.Should().Contain("\n"); // Should contain line breaks
    }

    [Theory]
    [InlineData("definition.json")]
    [InlineData("api-config.json")]
    [InlineData("mock-data.json")]
    public void GenerateDefinition_WithDifferentFileNames_ShouldWork(string fileName)
    {
        // Arrange
        var definitionFile = Path.Combine(_testFilesDirectory, fileName);

        // Act
        _definitionGenerator.GenerateDefinition(definitionFile);

        // Assert
        File.Exists(definitionFile).Should().BeTrue();
        var jsonContent = File.ReadAllText(definitionFile);
        jsonContent.Should().NotBeNullOrEmpty();
    }
}