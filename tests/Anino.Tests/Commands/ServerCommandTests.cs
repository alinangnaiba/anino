using Anino.Commands;
using Anino.Services;
using FluentAssertions;
using NSubstitute;

namespace Anino.Tests.Commands;

public class ServerCommandTests
{
    private readonly IAninoApplication _mockApplication;
    private readonly ServerCommand _serverCommand;

    public ServerCommandTests()
    {
        _mockApplication = Substitute.For<IAninoApplication>();
        _serverCommand = new ServerCommand(_mockApplication);
    }

    [Fact]
    public void CreateCommand_ShouldReturnCommandWithCorrectName()
    {
        // Act
        var command = _serverCommand.CreateCommand();

        // Assert
        command.Name.Should().Be("server");
        command.Description.Should().Be("Start the Anino mock API server");
    }

    [Fact]
    public void CreateCommand_ShouldHaveUsingOption()
    {
        // Act
        var command = _serverCommand.CreateCommand();

        // Assert
        var usingOption = command.Options.FirstOrDefault(o => o.Name == "--def");
        usingOption.Should().NotBeNull();
        usingOption!.Description.Should().Be("Path to JSON configuration file");
        usingOption.Aliases.Should().Contain("-d");
    }

    [Fact]
    public void CreateCommand_ShouldHavePortOption()
    {
        // Act
        var command = _serverCommand.CreateCommand();

        // Assert
        var portOption = command.Options.FirstOrDefault(o => o.Name == "--port");
        portOption.Should().NotBeNull();
        portOption!.Description.Should().Contain("Server port");
        portOption.Aliases.Should().Contain("-p");
    }

    [Fact]
    public void CreateCommand_ShouldHaveLatencyOption()
    {
        // Act
        var command = _serverCommand.CreateCommand();

        // Assert
        var latencyOption = command.Options.FirstOrDefault(o => o.Name == "--latency");
        latencyOption.Should().NotBeNull();
        latencyOption!.Description.Should().Contain("Simulated network latency");
        latencyOption.Aliases.Should().Contain("-l");
    }
}