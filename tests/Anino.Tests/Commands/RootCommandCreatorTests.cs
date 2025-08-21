using Anino.Commands;
using Anino.Services;
using FluentAssertions;
using NSubstitute;

namespace Anino.Tests.Commands;

public class RootCommandCreatorTests
{
    private readonly IAninoApplication _mockApplication;
    private readonly RootCommandCreator _rootCommandCreator;

    public RootCommandCreatorTests()
    {
        _mockApplication = Substitute.For<IAninoApplication>();
        _rootCommandCreator = new RootCommandCreator(_mockApplication);
    }

    [Fact]
    public void Create_ShouldReturnRootCommandWithCorrectDescription()
    {
        // Act
        var rootCommand = _rootCommandCreator.Create();

        // Assert
        rootCommand.Description.Should().Contain("Anino - Mock API Server");
        rootCommand.Description.Should().Contain("lightweight tool for creating mock REST APIs");
    }

    [Fact]
    public void Create_ShouldHaveServerSubcommand()
    {
        // Act
        var rootCommand = _rootCommandCreator.Create();

        // Assert
        var serverCommand = rootCommand.Subcommands.FirstOrDefault(s => s.Name == "server");
        serverCommand.Should().NotBeNull();
        serverCommand!.Description.Should().Be("Start the Anino mock API server");
    }

    [Fact]
    public void Create_ShouldHaveDefSubcommand()
    {
        // Act
        var rootCommand = _rootCommandCreator.Create();

        // Assert
        var defCommand = rootCommand.Subcommands.FirstOrDefault(s => s.Name == "def");
        defCommand.Should().NotBeNull();
        defCommand!.Description.Should().Be("Definition file operations");
    }

    [Fact]
    public void Create_ShouldHaveExactlyTwoSubcommands()
    {
        // Act
        var rootCommand = _rootCommandCreator.Create();

        // Assert
        rootCommand.Subcommands.Should().HaveCount(2);
        rootCommand.Subcommands.Select(s => s.Name).Should().Contain(new[] { "server", "def" });
    }
}