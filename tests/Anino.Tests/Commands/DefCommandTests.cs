using Anino.Commands;
using Anino.Services;
using FluentAssertions;
using NSubstitute;

namespace Anino.Tests.Commands;

public class DefCommandTests
{
    private readonly IAninoApplication _mockApplication;
    private readonly DefCommand _defCommand;

    public DefCommandTests()
    {
        _mockApplication = Substitute.For<IAninoApplication>();
        _defCommand = new DefCommand(_mockApplication);
    }

    [Fact]
    public void CreateCommand_ShouldReturnCommandWithCorrectName()
    {
        // Act
        var command = _defCommand.CreateCommand();

        // Assert
        command.Name.Should().Be("def");
        command.Description.Should().Be("Definition file operations");
    }

    [Fact]
    public void CreateCommand_ShouldHaveNewSubcommand()
    {
        // Act
        var command = _defCommand.CreateCommand();

        // Assert
        var newSubcommand = command.Subcommands.FirstOrDefault(s => s.Name == "new");
        newSubcommand.Should().NotBeNull();
        newSubcommand!.Description.Should().Be("Generate a new sample definition file with common CRUD operations");
    }
}