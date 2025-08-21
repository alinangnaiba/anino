using System.Text.Json;
using Anino.Configuration;
using Anino.Models;
using Anino.Services;
using FluentAssertions;
using NSubstitute;

namespace Anino.Tests.Services;

public class AninoApplicationTests
{
    private readonly IConfigurationLoader _mockConfigurationLoader;
    private readonly IMockServerBuilder _mockServerBuilder;
    private readonly IConsoleOutput _mockConsoleOutput;
    private readonly IDefinitionGenerator _mockTemplateGenerator;
    private readonly IAninoWebApplication _mockWebApp;
    private readonly AninoApplication _application;

    public AninoApplicationTests()
    {
        _mockConfigurationLoader = Substitute.For<IConfigurationLoader>();
        _mockServerBuilder = Substitute.For<IMockServerBuilder>();
        _mockConsoleOutput = Substitute.For<IConsoleOutput>();
        _mockTemplateGenerator = Substitute.For<IDefinitionGenerator>();
        _mockWebApp = Substitute.For<IAninoWebApplication>();

        _application = new AninoApplication(
            _mockConfigurationLoader,
            _mockServerBuilder,
            _mockConsoleOutput,
            _mockTemplateGenerator);
    }

    [Fact]
    public void Run_WithNullFile_ShouldReturnErrorCode()
    {
        // Arrange
        var options = new AninoOptions { File = null };

        // Act
        var result = _application.Run(options);

        // Assert
        result.Should().Be(1);
        _mockConsoleOutput.Received(1).WriteError("--def parameter is required.");
    }

    [Fact]
    public void Run_WithValidConfiguration_ShouldStartServerAndReturnSuccess()
    {
        // Arrange
        var testFile = new FileInfo("test.json");
        var options = new AninoOptions 
        { 
            File = testFile, 
            Port = 3000, 
            Latency = 500 
        };

        var endpoints = new List<ApiEndpoint>
        {
            new()
            {
                Path = "/api/test",
                Method = "GET",
                StatusCode = 200,
                Response = JsonDocument.Parse("{\"test\": true}").RootElement
            }
        };

        _mockConfigurationLoader.LoadEndpoints(testFile).Returns(endpoints);
        _mockServerBuilder.BuildServer(endpoints, 500).Returns(_mockWebApp);

        // Act
        var result = _application.Run(options);

        // Assert
        result.Should().Be(0);
        _mockConsoleOutput.Received(1).WriteStartupMessage();
        _mockConsoleOutput.Received(1).WriteLatencyMessage(500);
        _mockConsoleOutput.Received(1).WriteEndpointMapped("GET", "/api/test");
        _mockConsoleOutput.Received(1).WriteServerRunning(3000);
        _mockWebApp.Received(1).Run("http://localhost:3000");
    }

    [Fact]
    public void Run_WithoutLatency_ShouldNotWriteLatencyMessage()
    {
        // Arrange
        var testFile = new FileInfo("test.json");
        var options = new AninoOptions 
        { 
            File = testFile, 
            Port = 3000, 
            Latency = 0 
        };

        var endpoints = new List<ApiEndpoint>
        {
            new()
            {
                Path = "/api/test",
                Method = "GET",
                StatusCode = 200,
                Response = JsonDocument.Parse("{\"test\": true}").RootElement
            }
        };

        _mockConfigurationLoader.LoadEndpoints(testFile).Returns(endpoints);
        _mockServerBuilder.BuildServer(endpoints, 0).Returns(_mockWebApp);

        // Act
        var result = _application.Run(options);

        // Assert
        result.Should().Be(0);
        _mockConsoleOutput.DidNotReceive().WriteLatencyMessage(Arg.Any<int>());
    }

    [Fact]
    public void Run_WithFileNotFoundException_ShouldReturnErrorCode()
    {
        // Arrange
        var testFile = new FileInfo("nonexistent.json");
        var options = new AninoOptions { File = testFile };

        _mockConfigurationLoader.LoadEndpoints(testFile)
            .Returns(_ => throw new FileNotFoundException("File not found"));

        // Act
        var result = _application.Run(options);

        // Assert
        result.Should().Be(1);
        _mockConsoleOutput.Received(1).WriteError(Arg.Is<string>(s => s.Contains("File") && s.Contains("nonexistent.json") && s.Contains("not found")));
    }

    [Fact]
    public void Run_WithJsonException_ShouldReturnErrorCode()
    {
        // Arrange
        var testFile = new FileInfo("invalid.json");
        var options = new AninoOptions { File = testFile };

        _mockConfigurationLoader.LoadEndpoints(testFile)
            .Returns(_ => throw new JsonException("Invalid JSON"));

        // Act
        var result = _application.Run(options);

        // Assert
        result.Should().Be(1);
        _mockConsoleOutput.Received(1).WriteError(Arg.Is<string>(s => s.Contains("Invalid JSON format") && s.Contains("invalid.json") && s.Contains("Invalid JSON")));
    }

    [Fact]
    public void Run_WithGenerateTemplate_ShouldGenerateTemplateAndReturnSuccess()
    {
        // Arrange
        var templateFileName = "sample-template.json";
        var options = new AninoOptions 
        { 
            GenerateDefinition = templateFileName 
        };

        // Act
        var result = _application.Run(options);

        // Assert
        result.Should().Be(0);
        _mockTemplateGenerator.Received(1).GenerateDefinition(templateFileName);
    }

    [Fact]
    public void Run_WithGenerateTemplateDefaultName_ShouldGenerateTemplateWithDefaultName()
    {
        // Arrange
        var options = new AninoOptions 
        { 
            GenerateDefinition = "template.json" // This would come from processing the default
        };

        // Act
        var result = _application.Run(options);

        // Assert
        result.Should().Be(0);
        _mockTemplateGenerator.Received(1).GenerateDefinition("template.json");
    }

    [Theory]
    [InlineData("myconfig.json")]
    [InlineData("api-setup.json")]
    [InlineData("test-data.json")]
    public void Run_WithGenerateTemplateVariousNames_ShouldGenerateTemplateWithProvidedName(string templateFileName)
    {
        // Arrange
        var options = new AninoOptions 
        { 
            GenerateDefinition = templateFileName 
        };

        // Act
        var result = _application.Run(options);

        // Assert
        result.Should().Be(0);
        _mockTemplateGenerator.Received(1).GenerateDefinition(templateFileName);
    }
}