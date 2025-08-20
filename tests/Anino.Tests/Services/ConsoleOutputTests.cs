using Anino.Services;
using FluentAssertions;

namespace Anino.Tests.Services;

public class ConsoleOutputTests : IDisposable
{
    private readonly ConsoleOutput _consoleOutput;
    private readonly StringWriter _stringWriter;
    private readonly TextWriter _originalOutput;

    public ConsoleOutputTests()
    {
        _consoleOutput = new ConsoleOutput();
        _stringWriter = new StringWriter();
        _originalOutput = Console.Out;
        Console.SetOut(_stringWriter);
    }

    public void Dispose()
    {
        Console.SetOut(_originalOutput);
        _stringWriter.Dispose();
    }

    [Fact]
    public void WriteStartupMessage_ShouldWriteCorrectMessage()
    {
        // Act
        _consoleOutput.WriteStartupMessage();

        // Assert
        var output = _stringWriter.ToString();
        output.Should().Contain("--> Mapping endpoints...");
    }

    [Fact]
    public void WriteLatencyMessage_ShouldWriteCorrectMessage()
    {
        // Act
        _consoleOutput.WriteLatencyMessage(1000);

        // Assert
        var output = _stringWriter.ToString();
        output.Should().Contain("⏱️  Latency simulation enabled: 1000ms delay per request");
    }

    [Fact]
    public void WriteEndpointMapped_ShouldWriteCorrectMessage()
    {
        // Act
        _consoleOutput.WriteEndpointMapped("GET", "/api/users");

        // Assert
        var output = _stringWriter.ToString();
        output.Should().Contain("✓ Mapped [GET] /api/users");
    }

    [Fact]
    public void WriteServerRunning_ShouldWriteCorrectMessage()
    {
        // Act
        _consoleOutput.WriteServerRunning(3000);

        // Assert
        var output = _stringWriter.ToString();
        output.Should().Contain("🚀 Anino server is running. Listening on http://localhost:3000");
        output.Should().Contain("Press Ctrl+C to shut down.");
    }

    [Fact]
    public void WriteError_ShouldWriteCorrectMessage()
    {
        // Act
        _consoleOutput.WriteError("Test error message");

        // Assert
        var output = _stringWriter.ToString();
        output.Should().Contain("Error: Test error message");
    }
}