using System.Text.Json;
using Anino.Services;
using FluentAssertions;

namespace Anino.Tests.Services;

public class TemplateGeneratorTests : IDisposable
{
    private readonly TemplateGenerator _templateGenerator;
    private readonly string _testFilesDirectory;

    public TemplateGeneratorTests()
    {
        _templateGenerator = new TemplateGenerator();
        _testFilesDirectory = Path.Combine(Path.GetTempPath(), "AninoTemplateTests", Guid.NewGuid().ToString());
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
    public void GenerateTemplate_ShouldCreateValidJsonFile()
    {
        // Arrange
        var templateFile = Path.Combine(_testFilesDirectory, "test-template.json");

        // Act
        _templateGenerator.GenerateTemplate(templateFile);

        // Assert
        File.Exists(templateFile).Should().BeTrue();
        
        var jsonContent = File.ReadAllText(templateFile);
        jsonContent.Should().NotBeNullOrEmpty();
        
        // Verify it's valid JSON by deserializing
        var act = () => JsonSerializer.Deserialize<JsonElement[]>(jsonContent);
        act.Should().NotThrow();
    }

    [Fact]
    public void GenerateTemplate_ShouldContainCrudOperations()
    {
        // Arrange
        var templateFile = Path.Combine(_testFilesDirectory, "crud-template.json");

        // Act
        _templateGenerator.GenerateTemplate(templateFile);

        // Assert
        var jsonContent = File.ReadAllText(templateFile);
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
    public void GenerateTemplate_ShouldContainUsersEndpoints()
    {
        // Arrange
        var templateFile = Path.Combine(_testFilesDirectory, "users-template.json");

        // Act
        _templateGenerator.GenerateTemplate(templateFile);

        // Assert
        var jsonContent = File.ReadAllText(templateFile);
        var endpoints = JsonSerializer.Deserialize<JsonElement[]>(jsonContent);

        // Check that we have user-related endpoints
        var paths = endpoints
            .Select(e => e.GetProperty("path").GetString())
            .ToList();

        paths.Should().Contain("/api/users");
        paths.Should().Contain("/api/users/{id}");
    }

    [Fact]
    public void GenerateTemplate_ShouldHaveProperStatusCodes()
    {
        // Arrange
        var templateFile = Path.Combine(_testFilesDirectory, "status-template.json");

        // Act
        _templateGenerator.GenerateTemplate(templateFile);

        // Assert
        var jsonContent = File.ReadAllText(templateFile);
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
    public void GenerateTemplate_ShouldBeFormattedWithIndentation()
    {
        // Arrange
        var templateFile = Path.Combine(_testFilesDirectory, "formatted-template.json");

        // Act
        _templateGenerator.GenerateTemplate(templateFile);

        // Assert
        var jsonContent = File.ReadAllText(templateFile);
        
        // Check that the JSON is properly formatted (indented)
        jsonContent.Should().Contain("  "); // Should contain indentation
        jsonContent.Should().Contain("\n"); // Should contain line breaks
    }

    [Theory]
    [InlineData("template.json")]
    [InlineData("api-config.json")]
    [InlineData("mock-data.json")]
    public void GenerateTemplate_WithDifferentFileNames_ShouldWork(string fileName)
    {
        // Arrange
        var templateFile = Path.Combine(_testFilesDirectory, fileName);

        // Act
        _templateGenerator.GenerateTemplate(templateFile);

        // Assert
        File.Exists(templateFile).Should().BeTrue();
        var jsonContent = File.ReadAllText(templateFile);
        jsonContent.Should().NotBeNullOrEmpty();
    }
}